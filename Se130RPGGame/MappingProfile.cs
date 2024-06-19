using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Se130RPGGame.Data.Models;
using Se130RPGGame.Data.Models.DTO.Character;
using Se130RPGGame.Data.Models.DTO.Skill;
using Se130RPGGame.Data.Models.DTO.Weapon;

namespace Se130RPGGame
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Character,CharacterDTO>().ReverseMap();
            CreateMap<Character,CharacterCreateDTO>().ReverseMap();
            CreateMap<Character,CharacterUpdateDTO>().ReverseMap();
            CreateMap<Skill,SkillDTO>().ReverseMap();
            CreateMap<Weapon,WeaponDTO>().ReverseMap();
        }
    }
}
