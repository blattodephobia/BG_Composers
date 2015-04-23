namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MustChangePassword_Option : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MustChangePassword", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "MustChangePassword");
        }
    }
}
