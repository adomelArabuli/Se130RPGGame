﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Se130RPGGame.Data;
using Se130RPGGame.Data.Models;
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

		public async Task<ServiceResponse<CharacterDTO>> GetDetailsAsync(int id)
		{
			var response = new ServiceResponse<CharacterDTO>();

			try
			{
				var character = await _context.characters
				.Include(x => x.Weapon)
				.Include(x => x.Skills)
				.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == _helperService.GetUserId());

				if (character is null)
				{
					response.Message = "Character not found";
					response.Success = false;
					return response;
				}

				response.Data = _mapper.Map<CharacterDTO>(character);
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}
			return response;
		}

		public async Task<ServiceResponse<int>> AddAsync(CharacterCreateDTO dto)
		{
			var response = new ServiceResponse<int>();

			try
			{
				var character = _mapper.Map<Character>(dto);

				character.User = await _context.users.FirstOrDefaultAsync(x => x.Id == _helperService.GetUserId());

				var skills = await _context.skills.Where(x => dto.SkillIds.Contains(x.Id)).ToListAsync();

				character.Skills = skills;

				var weapon = await _context.weapons.FirstOrDefaultAsync(x => x.Id == dto.WeaponId);

				if (weapon is null) 
				{
					response.Success = false;
					response.Message = "Weapon you try to add to character not found";
					return response;
				}

				character.Weapon = weapon;

				await _context.characters.AddAsync(character);
				await _context.SaveChangesAsync();

				response.Data = character.Id;
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.GetFullMessage();
			}
			return response;
		}

		public async Task<ServiceResponse<bool>> UpdateAsync(CharacterUpdateDTO dto)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var characterToUpdate = await _context.characters
				.Include(x => x.Skills)
				.Include(x => x.User)
				.Include(x => x.Weapon)
				.FirstOrDefaultAsync(x => x.Id == dto.Id);

				if (characterToUpdate is null)
				{
					response.Message = "Character not found";
					response.Success = false;
					return response;
				}

				if (characterToUpdate.User.Id != _helperService.GetUserId())
				{
					response.Success = false;
					response.Message = "Character not found";
					return response;
				}

				var weapon = await _context.weapons.FirstOrDefaultAsync(x => x.Id == dto.WeaponId);

				if (weapon is null)
				{
					response.Success = false;
					response.Message = "Weapon you try to add to character not found";
					return response;
				}

				characterToUpdate.Weapon = weapon;

				_mapper.Map(dto, characterToUpdate);

				var skills = await _context.skills.Where(x => dto.SkillIds.Contains(x.Id)).ToListAsync();
				characterToUpdate.Skills = skills;

				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteAsync(int id)
		{
			var response = new ServiceResponse<bool>();

			try
			{
				var characterToDelete = await _context.characters
					.FirstOrDefaultAsync(x => x.Id == id && x.User.Id == _helperService.GetUserId());

				if (characterToDelete is null)
				{
					response.Message = "Character not found";
					response.Success = false;
					return response;
				}

				_context.characters.Remove(characterToDelete);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				response.Success = false;
				response.Message = ex.Message;
			}

			return response;
		}
	}
}
