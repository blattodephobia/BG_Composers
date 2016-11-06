namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BgcUser_PasswordResetTokenHash : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PasswordResetTokenHash", c => c.String(maxLength: 44, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PasswordResetTokenHash");
        }
    }
}
