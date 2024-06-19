using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Character;

namespace Se130RPGGame.Interfaces
{
	public interface ICharacterService
	{
		Task<ServiceResponse<ICollection<CharacterDTO>>> GetAllAsync();
		Task<ServiceResponse<CharacterDTO>> GetDetailsAsync(int id);
		Task<ServiceResponse<int>> AddAsync(CharacterCreateDTO dto);
		Task<ServiceResponse<bool>> UpdateAsync(CharacterUpdateDTO dto);
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
