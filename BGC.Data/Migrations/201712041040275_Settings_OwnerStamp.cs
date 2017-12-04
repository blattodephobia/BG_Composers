namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Settings_OwnerStamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Settings", "OwnerStamp", c => c.String(maxLength: 64, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Settings", "OwnerStamp");
        }
    }
}
