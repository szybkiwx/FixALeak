namespace FixALeak.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItineraryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Itineraries",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Itineraries");
        }
    }
}
