namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Glosssary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GlossaryEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GlossaryDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false, unicode: false),
                        Definition = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        GlossaryEntry_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GlossaryEntries", t => t.GlossaryEntry_Id)
                .Index(t => t.GlossaryEntry_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GlossaryDefinitions", "GlossaryEntry_Id", "dbo.GlossaryEntries");
            DropIndex("dbo.GlossaryDefinitions", new[] { "GlossaryEntry_Id" });
            DropTable("dbo.GlossaryDefinitions");
            DropTable("dbo.GlossaryEntries");
        }
    }
}
