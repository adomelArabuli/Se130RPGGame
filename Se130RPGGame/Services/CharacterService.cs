using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Character;
using Se130RPGGame.Interfaces;

namespace Se130RPGGame.Services
{
	public class CharacterService : ICharacterService
	{
		private readonly ApplicationDbContext _context;
		private readonly IHelperService _helperService;
		private readonly IMapper _mapper;
		public CharacterService(ApplicationDbContext context, IHelperService helperService, IMapper mapper)
		{
			_context = context;
			_helperService = helperService;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<ICollection<CharacterDTO>>> GetAllAsync()
		{
			var response = new ServiceResponse<ICollection<CharacterDTO>>();
			try
			{
				var characters = await _context.characters
					.Include(x => x.Weapon)
					.Include(x => x.Skills)
					.Where(x => x.User.Id == _helperService.GetUserId()).ToListAsync();

				response.Data = characters.Select(x => _mapper.Map<CharacterDTO>(x)).ToList();
				return response;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public Task<ServiceResponse<CharacterDTO>> GetDetailsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<int>> AddAsync(CharacterCreateDTO dto)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<bool>> UpdateAsync(CharacterUpdateDTO dto)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<bool>> DeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
	}
}
