namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitations_v2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Invitation_Id", c => c.Guid());
            CreateIndex("dbo.AspNetRoles", "Invitation_Id");
            AddForeignKey("dbo.AspNetRoles", "Invitation_Id", "dbo.Invitations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetRoles", "Invitation_Id", "dbo.Invitations");
            DropIndex("dbo.AspNetRoles", new[] { "Invitation_Id" });
            DropColumn("dbo.AspNetRoles", "Invitation_Id");
        }
    }
}
