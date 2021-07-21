namespace DemoStore_Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixBrand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        img = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Products", "CoverImage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CoverImage", c => c.String(nullable: false));
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropTable("dbo.ProductImages");
        }
    }
}
