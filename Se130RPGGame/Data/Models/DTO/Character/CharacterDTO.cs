using Se130RPGGame.Data.Models.DTO.Skill;
using Se130RPGGame.Data.Models.DTO.Weapon;
using Se130RPGGame.Enums;

namespace Se130RPGGame.Data.Models.DTO.Character
{
	public class CharacterDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Hitpoints { get; set; }
		public int Strength { get; set; }
		public int Defence { get; set; }
		public int Intelligence { get; set; }
		public RPGClass Class { get; set; }
		public WeaponDTO Weapon { get; set; }
		public ICollection<SkillDTO> Skills { get; set; }
		public int Fights { get; set; }
		public int Vitories { get; set; }
		public int Defeats { get; set; }
	}
}
