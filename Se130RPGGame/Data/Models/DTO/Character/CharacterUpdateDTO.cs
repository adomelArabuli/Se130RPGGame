using Se130RPGGame.Enums;

namespace Se130RPGGame.Data.Models.DTO.Character
{
	public class CharacterUpdateDTO
	{
        public int Id { get; set; }
        public string Name { get; set; }
		public int Hitpoints { get; set; }
		public int Strength { get; set; } 
		public int Defence { get; set; }
		public int Intelligence { get; set; }
		public RPGClass Class { get; set; }
		public ICollection<int> SkillIds { get; set; }
	}
}
