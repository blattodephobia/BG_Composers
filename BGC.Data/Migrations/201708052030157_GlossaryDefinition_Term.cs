namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GlossaryDefinition_Term : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GlossaryDefinitions", "Term", c => c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GlossaryDefinitions", "Term");
        }
    }
}
