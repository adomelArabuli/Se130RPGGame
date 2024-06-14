using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Se130RPGGame.Data.Models;

namespace Se130RPGGame.Data
{
    public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Skill>().HasData(
				new Skill { Id = 1, Name = "FireBall", Damage = 30 },
				new Skill { Id = 2, Name = "Frenzy", Damage = 20 },
				new Skill { Id = 3, Name = "Blizzard", Damage = 50 }
				);
		}

		public DbSet<User> users { get; set; }
		public DbSet<Role> roles { get; set; }
		//public DbSet<Character> characters { get; set; }
		public DbSet<Character> characters => Set<Character>();
		public DbSet<Weapon> weapons { get; set; }
		public DbSet<Skill> skills { get; set; }

    }
}
