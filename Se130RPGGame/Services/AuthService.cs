using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models;
using Se130RPGGame.Data.Models.DTO.Auth;
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
        private readonly IHelperService _helperService;
        public AuthService(ApplicationDbContext db, IConfiguration configuration, IHelperService helperService)
        {
            _db = db;
            _configuration = configuration;
            _helperService = helperService;
        }

        public async Task<ServiceResponse<int>> Register(UserRegisterDTO model)
        {
            var response = new ServiceResponse<int>();

            if (await UserExists(model.UserName))
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

            if (user is null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = await GenerateAccessToken(user);
                if (model.StaySignedIn)
                    await GenerateRefreshToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<AuthResultDTO>> RefreshAccessToken(string refreshToken)
        {
            var userId = GetUserId(refreshToken);

            var user = await _db.users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == userId);

            if (user is null)
                return new ServiceResponse<AuthResultDTO> { Success = false, Message = "User not found" };

            if (!user.RefreshToken.Equals(user.RefreshToken))
                return new ServiceResponse<AuthResultDTO> { Success = false, Message = "RefreshToken is not correct" };


            if (IsRefreshTokenExpired(user.RefreshToken))
                return new ServiceResponse<AuthResultDTO> { Success = false, Message = "RefreshToken is not correct" };

            var authResultDTO = await GenerateTokens(user);

            return new ServiceResponse<AuthResultDTO> { Data = authResultDTO };
        }

        #region LocalFunctions
        private async Task<bool> UserExists(string userName)
        {
            if (await _db.users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
            {
                return true;
            }
            return false;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hmac.Key;
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
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

        private async Task<string> GenerateRefreshToken(User user)
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
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            user.RefreshToken = token;

            await _db.SaveChangesAsync();

            return token;
        }

        private bool IsRefreshTokenExpired(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("Token:Secret").Value))
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null)
                    return true;

                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
                if (expClaim == null)
                    return true;

                var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));
                return expirationTime < DateTimeOffset.UtcNow;
            }
            catch (Exception)
            {
                return true;
            }
        }

        private async Task<AuthResultDTO> GenerateTokens(User user)
        {
            var refreshToken = await GenerateRefreshToken(user);

            var accessToken = await GenerateAccessToken(user);

            return new AuthResultDTO { AccessToken = accessToken, RefreshToken = refreshToken, Confirmed = true };
        }

        private int GetUserId(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
            int.TryParse(jsonToken.Claims.First(c => c.Type == "nameid").Value, out var userId);

            return userId;
        }
        #endregion
    }
}
