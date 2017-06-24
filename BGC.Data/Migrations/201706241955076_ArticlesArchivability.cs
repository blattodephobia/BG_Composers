namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticlesArchivability : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComposerArticles", "IsArchived", c => c.Boolean(nullable: false));
            AddColumn("dbo.ComposerArticles", "CreatedUtc", c => c.DateTime(nullable: false, precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComposerArticles", "CreatedUtc");
            DropColumn("dbo.ComposerArticles", "IsArchived");
        }
    }
}
