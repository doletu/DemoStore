namespace DemoStore_Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixProductMultipleImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "CoverImage", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "CoverImage");
        }
    }
}
