namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Permission_Refactoring : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Permissions", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Permissions", "Name", c => c.String(nullable: false, unicode: false));
        }
    }
}
