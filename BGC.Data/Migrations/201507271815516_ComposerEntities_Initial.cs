namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComposerEntities_Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComposerEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CultureName = c.String(unicode: false),
                        ArticleId = c.Guid(nullable: false),
                        ComposerId = c.Long(),
                        ComposerNameId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.ComposerId)
                .ForeignKey("dbo.ComposerNames", t => t.ComposerNameId)
                .Index(t => t.ComposerId)
                .Index(t => t.ComposerNameId);
            
            CreateTable(
                "dbo.Composers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComposerNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ComposerId = c.Long(),
                        LocalizationCultureName = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        CompleteName = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.ComposerId)
                .Index(t => t.ComposerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComposerEntries", "ComposerNameId", "dbo.ComposerNames");
            DropForeignKey("dbo.ComposerNames", "ComposerId", "dbo.Composers");
            DropForeignKey("dbo.ComposerEntries", "ComposerId", "dbo.Composers");
            DropIndex("dbo.ComposerNames", new[] { "ComposerId" });
            DropIndex("dbo.ComposerEntries", new[] { "ComposerNameId" });
            DropIndex("dbo.ComposerEntries", new[] { "ComposerId" });
            DropTable("dbo.ComposerNames");
            DropTable("dbo.Composers");
            DropTable("dbo.ComposerEntries");
        }
    }
}
