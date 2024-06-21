using Se130RPGGame.Enums;

namespace Se130RPGGame.Data.Models
{
	public class Character
	{
        public int Id { get; set; }
        public string Name { get; set; } = "Drako malfoi";
        public int Hitpoints { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
		public int Intelligence { get; set; } = 10;
        public int WeaponId { get; set; }
        public RPGClass Class { get; set; } = RPGClass.Knight;
        public User User { get; set; }
        public Weapon Weapon { get; set; }  
        public ICollection<Skill> Skills { get; set; }
        public int Fights { get; set; }
        public int Vitories { get; set; }
        public int Defeats { get; set; }
    }
}
