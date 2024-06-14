using Se130RPGGame.Enums;

namespace Se130RPGGame.Data.Models.DTO.Character
{
	public class CharacterCreateDTO
	{
		public string Name { get; set; } = "Drako malfoi";
		public int Hitpoints { get; set; } = 100;
		public int Strength { get; set; } = 10;
		public int Defence { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
		public RPGClass Class { get; set; } = RPGClass.Knight;
		public ICollection<int> SkillIds { get; set; }
	}
}
