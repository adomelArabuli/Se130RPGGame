using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Fight;
using Se130RPGGame.Data.Models.DTO.Fight.Skill;
using Se130RPGGame.Data.Models.DTO.Fight.Weapon;

namespace Se130RPGGame.Interfaces
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResponseDTO>> WeaponAttack(WeaponAttackRequestDTO request);
        Task<ServiceResponse<AttackResponseDTO>> SkillAttack(SkillAttackRequestDTO request);
        Task<ServiceResponse<FightResponseDTO>> Fight(FightRequestDTO request);
        //Task<ServiceResponse<AttackResponseDTO>> GetHighScore(WeaponAttackRequestDTO request);

        // GetHighScore


    }
}
