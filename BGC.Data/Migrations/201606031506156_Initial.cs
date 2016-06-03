namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
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
                    FirstName = c.String(maxLength: 32, storeType: "nvarchar"),
                    LastName = c.String(maxLength: 32, storeType: "nvarchar"),
                    FullName = c.String(maxLength: 128, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.ComposerId)
                .Index(t => t.ComposerId)
                .Index(t => t.FirstName)
                .Index(t => t.LastName)
                .Index(t => t.FullName);

            CreateTable(
                "dbo.AspNetRoles",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.Permissions",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(unicode: false),
                    Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                {
                    UserId = c.Long(nullable: false),
                    RoleId = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.AspNetUsers",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MustChangePassword = c.Boolean(nullable: false),
                    Email = c.String(maxLength: 256, storeType: "nvarchar"),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(unicode: false),
                    SecurityStamp = c.String(unicode: false),
                    PhoneNumber = c.String(unicode: false),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(precision: 0),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.Long(nullable: false),
                    ClaimType = c.String(unicode: false),
                    ClaimValue = c.String(unicode: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                {
                    UserId = c.Long(nullable: false),
                    ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    LoginProvider = c.String(unicode: false),
                })
                .PrimaryKey(t => new { t.UserId, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.Settings",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(unicode: false),
                    Description = c.String(unicode: false),
                    StringValue = c.String(unicode: false),
                    Priority = c.Int(nullable: false),
                    Date = c.DateTime(precision: 0),
                    Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.BgcRolePermissions",
                c => new
                {
                    BgcRole_Id = c.Long(nullable: false),
                    Permission_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.BgcRole_Id, t.Permission_Id })
                .ForeignKey("dbo.AspNetRoles", t => t.BgcRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.Permissions", t => t.Permission_Id, cascadeDelete: true)
                .Index(t => t.BgcRole_Id)
                .Index(t => t.Permission_Id);

            CreateTable(
                "dbo.BgcUserSettings",
                c => new
                {
                    BgcUser_Id = c.Long(nullable: false),
                    Setting_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.BgcUser_Id, t.Setting_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.BgcUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Settings", t => t.Setting_Id, cascadeDelete: true)
                .Index(t => t.BgcUser_Id)
                .Index(t => t.Setting_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.BgcUserSettings", "Setting_Id", "dbo.Settings");
            DropForeignKey("dbo.BgcUserSettings", "BgcUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BgcRolePermissions", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.BgcRolePermissions", "BgcRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.ComposerEntries", "ComposerNameId", "dbo.ComposerNames");
            DropForeignKey("dbo.ComposerNames", "ComposerId", "dbo.Composers");
            DropForeignKey("dbo.ComposerEntries", "ComposerId", "dbo.Composers");
            DropIndex("dbo.BgcUserSettings", new[] { "Setting_Id" });
            DropIndex("dbo.BgcUserSettings", new[] { "BgcUser_Id" });
            DropIndex("dbo.BgcRolePermissions", new[] { "Permission_Id" });
            DropIndex("dbo.BgcRolePermissions", new[] { "BgcRole_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ComposerNames", new[] { "FullName" });
            DropIndex("dbo.ComposerNames", new[] { "LastName" });
            DropIndex("dbo.ComposerNames", new[] { "FirstName" });
            DropIndex("dbo.ComposerNames", new[] { "ComposerId" });
            DropIndex("dbo.ComposerEntries", new[] { "ComposerNameId" });
            DropIndex("dbo.ComposerEntries", new[] { "ComposerId" });
            DropTable("dbo.BgcUserSettings");
            DropTable("dbo.BgcRolePermissions");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ComposerNames");
            DropTable("dbo.Composers");
            DropTable("dbo.ComposerEntries");
        }
    }
}
