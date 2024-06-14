using Microsoft.AspNetCore.Mvc;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.User;
using Se130RPGGame.Interfaces;

namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("Register")]
		public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO model)
		{
			var result = await _authService.Register(model);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPost("Login")]
		public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDTO model)
		{
			var result = await _authService.Login(model);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
