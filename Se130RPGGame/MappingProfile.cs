using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Se130RPGGame.Data.Models;
using Se130RPGGame.Data.Models.DTO.Character;

namespace Se130RPGGame
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<Character,CharacterDTO>().ReverseMap();
        }
    }
}
