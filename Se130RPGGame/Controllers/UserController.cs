using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.User;
using Se130RPGGame.Interfaces;

namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<ICollection<UserDTO>>>> GetUsersAsunc()
		{
			var result = await _userService.GetAllUsersAsync();
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<UserDetailsDTO>>> GetUserAsync(int id)
		{
			var result = await _userService.GetUserAsync(id);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<int>>> CreateUserAsync(UserCreateDTO model)
		{
			var result = await _userService.CreateUserAsync(model);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserAsync(UserUpdateDTO model)
		{
			var result = await _userService.UpdateUserAsync(model);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteUserAsunc(int id)
		{
			var result = await _userService.DeleteUserAsync(id);
			if (!result.Success)
			{
				return BadRequest(result);
			}
			return Ok(result);
		}
	}
}
