namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MediaTypeInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaTypeInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MimeTypeInternal = c.String(nullable: false, unicode: false),
                        StorageId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StorageId);
            
            CreateTable(
                "dbo.MediaTypeInfoComposerArticles",
                c => new
                    {
                        MediaTypeInfo_Id = c.Long(nullable: false),
                        ComposerArticle_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MediaTypeInfo_Id, t.ComposerArticle_Id })
                .ForeignKey("dbo.MediaTypeInfoes", t => t.MediaTypeInfo_Id, cascadeDelete: true)
                .ForeignKey("dbo.ComposerArticles", t => t.ComposerArticle_Id, cascadeDelete: true)
                .Index(t => t.MediaTypeInfo_Id)
                .Index(t => t.ComposerArticle_Id);
            
            CreateIndex("dbo.ComposerArticles", "StorageId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MediaTypeInfoComposerArticles", "ComposerArticle_Id", "dbo.ComposerArticles");
            DropForeignKey("dbo.MediaTypeInfoComposerArticles", "MediaTypeInfo_Id", "dbo.MediaTypeInfoes");
            DropIndex("dbo.MediaTypeInfoComposerArticles", new[] { "ComposerArticle_Id" });
            DropIndex("dbo.MediaTypeInfoComposerArticles", new[] { "MediaTypeInfo_Id" });
            DropIndex("dbo.MediaTypeInfoes", new[] { "StorageId" });
            DropIndex("dbo.ComposerArticles", new[] { "StorageId" });
            DropTable("dbo.MediaTypeInfoComposerArticles");
            DropTable("dbo.MediaTypeInfoes");
        }
    }
}
