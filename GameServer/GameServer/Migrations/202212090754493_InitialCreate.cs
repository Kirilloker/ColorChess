namespace GameServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Int(nullable: false),
                        Player1Score = c.Int(nullable: false),
                        Player2Score = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User1Id = c.Int(nullable: false),
                        User2Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lobbies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameMode = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Win = c.Int(nullable: false),
                        Lose = c.Int(nullable: false),
                        MaxScore = c.Int(nullable: false),
                        Draw = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserStatistics");
            DropTable("dbo.Users");
            DropTable("dbo.Lobbies");
            DropTable("dbo.GameStatistics");
        }
    }
}
