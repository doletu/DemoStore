namespace DemoStore_Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Role1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.RoleModels", newName: "RoleViewModels");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.RoleViewModels", newName: "RoleModels");
        }
    }
}
