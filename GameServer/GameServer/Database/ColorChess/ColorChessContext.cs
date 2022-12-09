using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace FirstEF6App
{
	class ColorChessContext : DbContext
	{
		public ColorChessContext()
			: base("DbConnection")
		{ }

		public DbSet<User> Users { get; set; }
		public DbSet<GameStatistic> GameStatistics { get; set; }
		public DbSet<UserStatistic> UserStatistics { get; set; }
		public DbSet<Lobby> Lobbies { get; set; }
		public DbSet<Room> Rooms { get; set; }
	}
}