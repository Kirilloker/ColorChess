using GameServer;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace FirstEF6App
{
    public class ColorChessContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server="   + Config.IpDB           + ";" + 
                "user="     + Config.UserDB         + ";" + 
                "password=" + Config.PasswordDB     + ";" +
                "database=" + Config.NameDB          + ";",
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


            modelBuilder
                .Entity<LogEvent>()
                .Property(e => e.UsersId)
                .HasConversion(new IntListToStringConverter());
        }


        public ColorChessContext()
        {
            Database.EnsureCreated();
        }

        public List<T> ExecuteSqlQuery<T>(string sqlQuery) where T : class
        {
            return Set<T>().FromSqlRaw(sqlQuery).ToList();
        }

        public void ExecuteSqlQuery2(string sqlQuery)
        {
            var result = Database.ExecuteSqlRaw(sqlQuery);
        }


        public DbSet<User> users { get; set; }
		public DbSet<GameStatistic> gamestatistics { get; set; }
		public DbSet<UserStatistic> userstatistics { get; set; }
		public DbSet<LogEvent> logevents { get; set; }
	}
}