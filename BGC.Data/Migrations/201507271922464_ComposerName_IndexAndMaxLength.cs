namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComposerName_IndexAndMaxLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComposerNames", "FirstName", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.ComposerNames", "LastName", c => c.String(maxLength: 32, storeType: "nvarchar"));
            AlterColumn("dbo.ComposerNames", "CompleteName", c => c.String(maxLength: 128, storeType: "nvarchar"));
            CreateIndex("dbo.ComposerNames", "FirstName");
            CreateIndex("dbo.ComposerNames", "LastName");
            CreateIndex("dbo.ComposerNames", "CompleteName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ComposerNames", new[] { "CompleteName" });
            DropIndex("dbo.ComposerNames", new[] { "LastName" });
            DropIndex("dbo.ComposerNames", new[] { "FirstName" });
            AlterColumn("dbo.ComposerNames", "CompleteName", c => c.String(unicode: false));
            AlterColumn("dbo.ComposerNames", "LastName", c => c.String(unicode: false));
            AlterColumn("dbo.ComposerNames", "FirstName", c => c.String(unicode: false));
        }
    }
}
