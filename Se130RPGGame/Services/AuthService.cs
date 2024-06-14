using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models;
using Se130RPGGame.Data.Models.DTO.User;
using Se130RPGGame.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Se130RPGGame.Services
{
    public class AuthService : IAuthService
	{
		private readonly ApplicationDbContext _db;
		private readonly IConfiguration _configuration;
		public AuthService(ApplicationDbContext db, IConfiguration configuration)
		{
			_db = db;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<int>> Register(UserRegisterDTO model)
		{
			var response = new ServiceResponse<int>();

			if(await UserExists(model.UserName))
			{
				response.Success = false;
				response.Message = "User already exists";
				return response;
			}

			CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

			var user = new User()
			{
				UserName = model.UserName,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};

			await _db.users.AddAsync(user);
			await _db.SaveChangesAsync();

			response.Data = user.Id;
			return response;
		}

		public async Task<ServiceResponse<string>> Login(UserLoginDTO model)
		{
			var response = new ServiceResponse<string>();

			var user = await _db.users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.UserName.ToLower() == model.UserName.ToLower());

			if(user is null)
			{
				response.Success = false;
				response.Message = "User not found";
			}
			else if(!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
			{
				response.Success = false;
				response.Message = "Wrong password";
			}
			else
			{
				response.Data = await GenerateAccessToken(user);
			}

			return response;
		}

		private async Task<bool> UserExists(string userName)
		{
			if(await _db.users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
			{
				return true;
			}
			return false;
		}

		public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using(var hmac = new HMACSHA512())
			{
				passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				passwordSalt = hmac.Key;
			}
		}
		
		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using(var hmac = new HMACSHA512(passwordSalt))
			{
				var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				return computeHash.SequenceEqual(passwordHash);
			}
		}
			 
		private async Task<string> GenerateAccessToken(User user)
		{
			var rolesIds = user.Roles.Select(x => x.Id);
			var roleNames = await _db.roles.Where(x => rolesIds.Contains(x.Id)).Select(x => x.Name).ToListAsync();

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.UserName)
			};

			foreach (var roleName in roleNames)
			{
				claims.Add(new Claim(ClaimTypes.Role, roleName));
			}

			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
				.GetBytes(_configuration.GetSection("Token:Secret").Value));

			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(1),
				SigningCredentials = credentials
			};
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
