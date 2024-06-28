using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Auth;
using Se130RPGGame.Data.Models.DTO.User;

namespace Se130RPGGame.Interfaces
{
    public interface IAuthService
	{
		Task<ServiceResponse<int>> Register(UserRegisterDTO model);
		Task<ServiceResponse<string>> Login(UserLoginDTO model);
		Task<ServiceResponse<AuthResultDTO>> RefreshAccessToken(string refreshToken);
	}
}
