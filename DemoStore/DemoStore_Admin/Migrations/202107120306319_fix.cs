namespace DemoStore_Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductImages", "img", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductImages", "img", c => c.Binary());
        }
    }
}
