namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class ComposerProfile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComposerProfiles",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ComposerProfileMediaTypeInfoes",
                c => new
                {
                    ComposerProfile_Id = c.Guid(nullable: false),
                    MediaTypeInfo_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.ComposerProfile_Id, t.MediaTypeInfo_Id })
                .ForeignKey("dbo.ComposerProfiles", t => t.ComposerProfile_Id, cascadeDelete: true)
                .ForeignKey("dbo.MediaTypeInfos", t => t.MediaTypeInfo_Id, cascadeDelete: true)
                .Index(t => t.ComposerProfile_Id)
                .Index(t => t.MediaTypeInfo_Id);

            AddColumn("dbo.Composers", "Profile_Id", c => c.Guid());
            AddColumn("dbo.MediaTypeInfos", "ComposerProfile_Id", c => c.Guid());
            CreateIndex("dbo.Composers", "Profile_Id");
            CreateIndex("dbo.MediaTypeInfos", "ComposerProfile_Id");
            AddForeignKey("dbo.MediaTypeInfos", "ComposerProfile_Id", "dbo.ComposerProfiles", "Id");
            AddForeignKey("dbo.Composers", "Profile_Id", "dbo.ComposerProfiles", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Composers", "Profile_Id", "dbo.ComposerProfiles");
            DropForeignKey("dbo.ComposerProfileMediaTypeInfoes", "MediaTypeInfo_Id", "dbo.MediaTypeInfos");
            DropForeignKey("dbo.ComposerProfileMediaTypeInfoes", "ComposerProfile_Id", "dbo.ComposerProfiles");
            DropForeignKey("dbo.MediaTypeInfos", "ComposerProfile_Id", "dbo.ComposerProfiles");
            DropIndex("dbo.ComposerProfileMediaTypeInfoes", new[] { "MediaTypeInfo_Id" });
            DropIndex("dbo.ComposerProfileMediaTypeInfoes", new[] { "ComposerProfile_Id" });
            DropIndex("dbo.MediaTypeInfos", new[] { "ComposerProfile_Id" });
            DropIndex("dbo.Composers", new[] { "Profile_Id" });
            DropColumn("dbo.MediaTypeInfos", "ComposerProfile_Id");
            DropColumn("dbo.Composers", "Profile_Id");
            DropTable("dbo.ComposerProfileMediaTypeInfoes");
            DropTable("dbo.ComposerProfiles");
        }
    }
}
