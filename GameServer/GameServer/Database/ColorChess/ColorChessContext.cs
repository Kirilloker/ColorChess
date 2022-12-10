using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FirstEF6App
{
	class ColorChessContext : Microsoft.EntityFrameworkCore.DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=loki5566;database=usersdb5;",
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