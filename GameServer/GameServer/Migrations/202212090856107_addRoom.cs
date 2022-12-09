namespace GameServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRoom : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Map = c.String(),
                        User1Id = c.Int(nullable: false),
                        User2Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rooms");
        }
    }
}
