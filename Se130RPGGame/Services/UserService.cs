using Se130RPGGame.Data.Models;
using Se130RPGGame.Data;
using Se130RPGGame.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Se130RPGGame.Data.Models.DTO.User;

namespace Se130RPGGame.Services
{
    public class UserService : IUserService
	{
		private readonly ApplicationDbContext _db;
		private readonly IMapper _mapper;
		public UserService(ApplicationDbContext db, IMapper mapper)
		{
			_db = db;
			_mapper = mapper;
		}

		public async Task<ServiceResponse<ICollection<UserDTO>>> GetAllUsersAsync()
		{
			try
			{
				var users = await _db.users.ToListAsync();

				return new() { Data = users.Select(x => _mapper.Map<UserDTO>(x)).ToList() };
			}
			catch (Exception ex)
			{
				return new() { Success = false, Message = ex.Message };
			}
		}

		public async Task<ServiceResponse<UserDetailsDTO>> GetUserAsync(int id)
		{
			try
			{
				var userToEdit = await _db.users.Include(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

				if (userToEdit == null)
				{
					return new() { Success = false, Message = "User not found" };
				}

				return new()
				{
					Data = _mapper.Map<User, UserDetailsDTO>(userToEdit, opt =>
				opt.AfterMap((src, dst) =>
				{
					src.Roles.ToList().ForEach(x =>
					{
						dst.RoleNames.Add(x.Name);
					});
				})),
					Success = true
				};
			}
			catch (Exception ex)
			{
				return new() { Success = false, Message = ex.Message };
			}
		}

		public async Task<ServiceResponse<int>> CreateUserAsync(UserCreateDTO model)
		{
			try
			{
				var response = new ServiceResponse<int>();
				var userExists = await _db.users.AnyAsync(x => x.UserName == model.UserName);
				if (userExists)
					return new() { Success = false, Message = "User already exists" };

				AuthService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

				var user = new User()
				{
					UserName = model.UserName,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt
				};

				var roles = await _db.roles.Where(x => model.Roles.Contains(x.Id)).ToListAsync();
				user.Roles = roles;

				await _db.users.AddAsync(user);
				await _db.SaveChangesAsync();

				response.Data = user.Id;
				return response;
			}
			catch (Exception ex)
			{
				return new() { Success = false, Message = ex.Message };
			}
		}

		public async Task<ServiceResponse<bool>> UpdateUserAsync(UserUpdateDTO model)
		{
			try
			{
				var response = new ServiceResponse<bool>();
				var userExists = await _db.users.AnyAsync(x => x.UserName == model.UserName);
				if (!userExists)
					return new() { Success = false, Message = "User not found" };

				var user = await _db.users.FirstOrDefaultAsync(x => x.UserName == model.UserName);
				if (model.Password != null)
				{
					AuthService.CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);
					user.PasswordHash = passwordHash;
					user.PasswordSalt = passwordSalt;
				}

				if (model.UserName != null)
					user.UserName = model.UserName;


				var roles = await _db.roles.Where(x => model.Roles.Contains(x.Id)).ToListAsync();
				user.Roles = roles;

				_db.users.Update(user);
				await _db.SaveChangesAsync();

				return new() { Data = true, Success = true };
			}
			catch (Exception ex)
			{
				return new() { Success = false, Message = ex.Message };
			}
		}

		public async Task<ServiceResponse<bool>> DeleteUserAsync(int id)
		{
			try
			{
				var userToDelete = await _db.users.FirstOrDefaultAsync(x => x.Id == id);
				if (userToDelete is null)
					return new() { Success = false, Message = "User not found" };

				_db.users.Remove(userToDelete);
				await _db.SaveChangesAsync();

				return new() { Success = true };

			}
			catch (Exception ex)
			{
				return new() { Success = false, Message = ex.Message };
			}
		}
	}
}
