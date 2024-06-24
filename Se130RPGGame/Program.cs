using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models.DTO.Character;
using Se130RPGGame.Interfaces;
using Se130RPGGame.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Se130RPGGame
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();

			builder.Services.AddControllers().AddFluentValidation(option =>
			option.RegisterValidatorsFromAssemblyContaining<CharacterCreateValidator>()
			);

			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Description = "Standard authorization header using the bearer scheme, e.g \"bearer {token} \"",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey
				});

				c.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			builder.Services.AddHttpContextAccessor();
			builder.Services.AddAutoMapper(typeof(Program).Assembly);

			builder.Services.AddDbContext<ApplicationDbContext>(option =>
			option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<IAuthService, AuthService>();
			builder.Services.AddScoped<ICharacterService, CharacterService>();
			builder.Services.AddScoped<IHelperService, HelperService>();
			builder.Services.AddScoped<IFightService, FightService>();

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
					.GetBytes(builder.Configuration.GetSection("Token:Secret").Value)),
					ValidateIssuer = false,
					ValidateAudience = false
				});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthentication();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}