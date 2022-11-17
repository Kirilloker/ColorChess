using System.Data.Entity;

namespace FirstEF6App
{
    class ColorChessContext : DbContext
    {
        public ColorChessContext() : base("DbConnection")
        {

        }

        public DbSet<Player> Players { get; set; }
        public DbSet<GameStatistic> GameStatistics { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
    }
}