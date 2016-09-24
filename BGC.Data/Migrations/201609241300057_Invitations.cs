namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invitations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false, unicode: false),
                        ExpirationDate = c.DateTime(nullable: false, precision: 0),
                        Sender_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id, cascadeDelete: true)
                .Index(t => t.Sender_Id);
            
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "Sender_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Invitations", new[] { "Sender_Id" });
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropTable("dbo.Invitations");
        }
    }
}
