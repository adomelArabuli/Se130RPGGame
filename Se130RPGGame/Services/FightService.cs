using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models;
using Se130RPGGame.Data.Models.DTO.Fight;
using Se130RPGGame.Data.Models.DTO.Fight.Skill;
using Se130RPGGame.Data.Models.DTO.Fight.Weapon;
using Se130RPGGame.Interfaces;

namespace Se130RPGGame.Services
{
    public class FightService : IFightService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public FightService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AttackResponseDTO>> WeaponAttack(WeaponAttackRequestDTO request)
        {
            var response = new ServiceResponse<AttackResponseDTO>();
            try
            {
                var attacker = await _context.characters
                    .Include(x => x.Weapon)
                    .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                var opponent = await _context.characters
                    .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.Hitpoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                FillResponse(response, attacker, opponent, damage);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        public async Task<ServiceResponse<AttackResponseDTO>> SkillAttack(SkillAttackRequestDTO request)
        {
            var response = new ServiceResponse<AttackResponseDTO>();

            try
            {
                var attacker = await _context.characters
                    .Include(x => x.Skills)
                    .FirstOrDefaultAsync(x => x.Id == request.AttackerId);

                var opponent = await _context.characters
                    .FirstOrDefaultAsync(x => x.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(x => x.Id == request.SkillId);

                if(skill is null)
                {
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesn't know that skill";
                }

                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.Hitpoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await _context.SaveChangesAsync();

                FillResponse(response, attacker, opponent, damage);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<FightResponseDTO>> Fight(FightRequestDTO request)
        {
            var response = new ServiceResponse<FightResponseDTO>()
            {
                Data = new FightResponseDTO()
            };

            try
            {
                var characters = await _context.characters
                    .Include(x => x.Weapon)
                    .Include(x => x.Skills)
                    .Where(x => request.CharacterIds.Contains(x.Id)).ToListAsync();

                bool isDefeated = false;

                while (!isDefeated)
                {
                    foreach (var attacker in characters)
                    {
                        var opponents = characters.Where(x => x.Id != attacker.Id).ToList();

                        var randomOpponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;

                        string attackUsed = string.Empty;

                        bool useWeapon = new Random().Next(2) == 0;

                        if (useWeapon)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, randomOpponent);
                        }
                        else
                        {
                            var skill = attacker.Skills.ToList()[new Random().Next(attacker.Skills.Count)];
                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, randomOpponent, skill);
                        }

                        response.Data.Log.
                            Add($"{attacker.Name} attacks {randomOpponent.Name} using {attackUsed} with {(damage >= 0 ? damage : 0)} damage.");

                        if(randomOpponent.Hitpoints <= 0)
                        {
                            // |= es ras nishnavs
                            isDefeated |= true;
                            attacker.Vitories++;
                            randomOpponent.Defeats++;
                            response.Data.Log.Add($"{randomOpponent.Name} hase been defeated");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.Hitpoints} hp left");
                            break;
                        }
                    }
                }

                characters.ForEach(x =>
                {
                    x.Fights++;
                    x.Hitpoints = 100;
                });

                await _context.SaveChangesAsync();  
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        #region LocalFunctions
        private static int DoWeaponAttack(Character? attacker, Character? opponent)
        {
            int damage = attacker.Weapon.Damage + new Random().Next(attacker.Strength);

            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
                opponent.Hitpoints -= damage;
            return damage;
        }

        private static int DoSkillAttack(Character? attacker, Character? opponent, Skill? skill)
        {
            int damage = skill.Damage + new Random().Next(attacker.Intelligence);

            damage -= new Random().Next(opponent.Defence);

            if (damage > 0)
                opponent.Hitpoints -= damage;
            return damage;
        }

        private static void FillResponse(ServiceResponse<AttackResponseDTO> response, Character? attacker, Character? opponent, int damage)
        {
            response.Data = new AttackResponseDTO
            {
                Attacker = attacker.Name,
                Opponent = opponent.Name,
                AttackerHP = attacker.Hitpoints,
                OpponentHP = opponent.Hitpoints,
                Damage = damage
            };
        }

        #endregion
    }
}
