namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settings_RequiredName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Settings", "Name", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Settings", "Name", c => c.String(unicode: false));
        }
    }
}
