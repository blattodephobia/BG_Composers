namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Composer_DatesAndOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Composers", "DateOfBirth", c => c.DateTime(precision: 0));
            AddColumn("dbo.Composers", "DateOfDeath", c => c.DateTime(precision: 0));
            AddColumn("dbo.Composers", "Order", c => c.Int(nullable: false));
            AddColumn("dbo.Composers", "HasNamesakes", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Composers", "HasNamesakes");
            DropColumn("dbo.Composers", "Order");
            DropColumn("dbo.Composers", "DateOfDeath");
            DropColumn("dbo.Composers", "DateOfBirth");
        }
    }
}
