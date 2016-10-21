namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitations_ObsoleteProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invitations", "IsObsolete", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Invitations", "IsObsolete");
        }
    }
}
