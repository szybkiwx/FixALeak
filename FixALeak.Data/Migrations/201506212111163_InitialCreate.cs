namespace FixALeak.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CategoryLeaves",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Expenses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Title = c.String(),
                        CategoryLeafID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CategoryLeaves", t => t.CategoryLeafID, cascadeDelete: true)
                .Index(t => t.CategoryLeafID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CategoryLeaves", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Expenses", "CategoryLeafID", "dbo.CategoryLeaves");
            DropIndex("dbo.Expenses", new[] { "CategoryLeafID" });
            DropIndex("dbo.CategoryLeaves", new[] { "CategoryID" });
            DropTable("dbo.Expenses");
            DropTable("dbo.CategoryLeaves");
            DropTable("dbo.Categories");
        }
    }
}
