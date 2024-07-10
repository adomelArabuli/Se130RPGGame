namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class CharacterController : ControllerBase
	{
		private readonly ICharacterService _characterService;

		public CharacterController(ICharacterService characterService)
		{
			_characterService = characterService;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAsync()
		{
			var result = await _characterService.GetAllAsync();
			if (result.Success) return Ok(result);
			return BadRequest(result.Message);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetDetailsAsync(int id)
		{
			var result = await _characterService.GetDetailsAsync(id);
			if (result.Success) return Ok(result);
			return BadRequest(result.Message);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAsync(CharacterCreateDTO dto)
		{
			var result = await _characterService.AddAsync(dto);
			if (result.Success) return Ok(result);
			return BadRequest(result.Message);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateAsync(CharacterUpdateDTO dto)
		{
			var result = await _characterService.UpdateAsync(dto);
			if (result.Success) return Ok(result);
			return BadRequest(result.Message);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteAsync(int id)
		{
			var result = await _characterService.DeleteAsync(id);
			if (result.Success) return Ok(result);
			return BadRequest(result.Message);
		}
	}
}
