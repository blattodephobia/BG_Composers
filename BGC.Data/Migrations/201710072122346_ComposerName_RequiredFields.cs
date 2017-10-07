namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComposerName_RequiredFields : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ComposerNames", new[] { "LastName" });
            DropIndex("dbo.ComposerNames", new[] { "FullName" });
            AlterColumn("dbo.ComposerNames", "LastName", c => c.String(nullable: false, maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.ComposerNames", "FullName", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            CreateIndex("dbo.ComposerNames", "LastName");
            CreateIndex("dbo.ComposerNames", "FullName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComposerNames", new[] { "FullName" });
            DropIndex("dbo.ComposerNames", new[] { "LastName" });
            AlterColumn("dbo.ComposerNames", "FullName", c => c.String(maxLength: 128, storeType: "nvarchar"));
            AlterColumn("dbo.ComposerNames", "LastName", c => c.String(maxLength: 32, storeType: "nvarchar"));
            CreateIndex("dbo.ComposerNames", "FullName");
            CreateIndex("dbo.ComposerNames", "LastName");
        }
    }
}
