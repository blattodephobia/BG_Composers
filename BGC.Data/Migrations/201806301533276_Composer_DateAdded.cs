namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Composer_DateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Composer", "DateAdded", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Composer", "DateAdded");
        }
    }
}
