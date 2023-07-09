using Microsoft.EntityFrameworkCore;

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

        public ColorChessContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
		public DbSet<GameStatistic> GameStatistics { get; set; }
		public DbSet<UserStatistic> UserStatistics { get; set; }
		public DbSet<Lobby> Lobbies { get; set; }
		public DbSet<Room> Rooms { get; set; }
	}
}