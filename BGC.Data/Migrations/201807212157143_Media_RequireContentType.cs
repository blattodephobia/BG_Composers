namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Media_RequireContentType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MediaTypeInfo", "MimeType", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MediaTypeInfo", "MimeType", c => c.String(unicode: false));
        }
    }
}
