using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.User;

namespace Se130RPGGame.Interfaces
{
    public interface IUserService
	{
		Task<ServiceResponse<ICollection<UserDTO>>> GetAllUsersAsync();
		Task<ServiceResponse<UserDetailsDTO>> GetUserAsync(int id);
		Task<ServiceResponse<int>> CreateUserAsync(UserCreateDTO model);
		Task<ServiceResponse<bool>> UpdateUserAsync(UserUpdateDTO model);
		Task<ServiceResponse<bool>> DeleteUserAsync(int id);
	}
}
