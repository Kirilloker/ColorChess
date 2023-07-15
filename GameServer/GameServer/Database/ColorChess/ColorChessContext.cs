using Microsoft.EntityFrameworkCore;
using static GameStatistic;

namespace FirstEF6App
{
    class ColorChessContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server="   + Config.IpDB           + ";" + 
                "user="     + Config.UserDB         + ";" + 
                "password=" + Config.PasswordDB     + ";" +
                "database=" +Config.NameDB          + ";",
                new MySqlServerVersion(new Version(8, 0, 13))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<GameStatistic>()
                .Property(e => e.PlayerScore)
                .HasConversion(new IntListToStringConverter());

            modelBuilder
                .Entity<GameStatistic>()
                .Property(e => e.UsersId)
                .HasConversion(new IntListToStringConverter());
        }


        public ColorChessContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
		public DbSet<GameStatistic> GameStatistics { get; set; }
		public DbSet<UserStatistic> UserStatistics { get; set; }
	}
}