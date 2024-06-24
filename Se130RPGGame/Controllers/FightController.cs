using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Fight;
using Se130RPGGame.Data.Models.DTO.Fight.Skill;
using Se130RPGGame.Data.Models.DTO.Fight.Weapon;
using Se130RPGGame.Interfaces;

namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FightController : ControllerBase
    {
        private readonly IFightService _fightService;

        public FightController(IFightService fightService)
        {
            _fightService = fightService;
        }

        [HttpPost("WeaponAttack")]
        public async Task<IActionResult> WeaponAttack(WeaponAttackRequestDTO request)
        {
            var result = await _fightService.WeaponAttack(request);
            if(result.Success) 
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("SkillAttack")]
        public async Task<IActionResult> SkillAttack(SkillAttackRequestDTO request)
        {
            var result = await _fightService.SkillAttack(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Fight")]
        public async Task<IActionResult> Fight(FightRequestDTO request)
        {
            var result = await _fightService.Fight(request);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
