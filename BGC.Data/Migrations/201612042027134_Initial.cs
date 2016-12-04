namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComposerArticles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        StorageId = c.Guid(nullable: false),
                        Composer_Id = c.Long(nullable: false),
                        LocalizedName_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.Composer_Id, cascadeDelete: true)
                .ForeignKey("dbo.ComposerNames", t => t.LocalizedName_Id, cascadeDelete: true)
                .Index(t => t.StorageId)
                .Index(t => t.Composer_Id)
                .Index(t => t.LocalizedName_Id);
            
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
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        FirstName = c.String(maxLength: 32, storeType: "nvarchar"),
                        LastName = c.String(maxLength: 32, storeType: "nvarchar"),
                        FullName = c.String(maxLength: 128, storeType: "nvarchar"),
                        Composer_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composers", t => t.Composer_Id, cascadeDelete: true)
                .Index(t => t.FirstName)
                .Index(t => t.LastName)
                .Index(t => t.FullName)
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "dbo.MediaTypeInfos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MimeTypeInternal = c.String(nullable: false, unicode: false),
                        StorageId = c.Guid(nullable: false),
                        OriginalFileName = c.String(nullable: false, maxLength: 255, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StorageId);
            
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Email = c.String(nullable: false, unicode: false),
                        ExpirationDate = c.DateTime(nullable: false, precision: 0),
                        IsObsolete = c.Boolean(nullable: false),
                        Sender_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Sender_Id, cascadeDelete: true)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, unicode: false),
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
                        PasswordResetTokenHash = c.String(maxLength: 44, unicode: false),
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
                        Description = c.String(maxLength: 1000, storeType: "nvarchar"),
                        StringValue = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Priority = c.Int(nullable: false),
                        Date = c.DateTime(precision: 0),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MediaTypeInfoComposerArticles",
                c => new
                    {
                        MediaTypeInfo_Id = c.Long(nullable: false),
                        ComposerArticle_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.MediaTypeInfo_Id, t.ComposerArticle_Id })
                .ForeignKey("dbo.MediaTypeInfos", t => t.MediaTypeInfo_Id, cascadeDelete: true)
                .ForeignKey("dbo.ComposerArticles", t => t.ComposerArticle_Id, cascadeDelete: true)
                .Index(t => t.MediaTypeInfo_Id)
                .Index(t => t.ComposerArticle_Id);
            
            CreateTable(
                "dbo.BgcRolePermission",
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
            
            CreateTable(
                "dbo.InvitationBgcRoles",
                c => new
                    {
                        Invitation_Id = c.Guid(nullable: false),
                        BgcRole_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Invitation_Id, t.BgcRole_Id })
                .ForeignKey("dbo.Invitations", t => t.Invitation_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.BgcRole_Id, cascadeDelete: true)
                .Index(t => t.Invitation_Id)
                .Index(t => t.BgcRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invitations", "Sender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.InvitationBgcRoles", "BgcRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.InvitationBgcRoles", "Invitation_Id", "dbo.Invitations");
            DropForeignKey("dbo.BgcUserSettings", "Setting_Id", "dbo.Settings");
            DropForeignKey("dbo.BgcUserSettings", "BgcUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BgcRolePermission", "Permission_Id", "dbo.Permissions");
            DropForeignKey("dbo.BgcRolePermission", "BgcRole_Id", "dbo.AspNetRoles");
            DropForeignKey("dbo.MediaTypeInfoComposerArticles", "ComposerArticle_Id", "dbo.ComposerArticles");
            DropForeignKey("dbo.MediaTypeInfoComposerArticles", "MediaTypeInfo_Id", "dbo.MediaTypeInfos");
            DropForeignKey("dbo.ComposerArticles", "LocalizedName_Id", "dbo.ComposerNames");
            DropForeignKey("dbo.ComposerArticles", "Composer_Id", "dbo.Composers");
            DropForeignKey("dbo.ComposerNames", "Composer_Id", "dbo.Composers");
            DropIndex("dbo.InvitationBgcRoles", new[] { "BgcRole_Id" });
            DropIndex("dbo.InvitationBgcRoles", new[] { "Invitation_Id" });
            DropIndex("dbo.BgcUserSettings", new[] { "Setting_Id" });
            DropIndex("dbo.BgcUserSettings", new[] { "BgcUser_Id" });
            DropIndex("dbo.BgcRolePermission", new[] { "Permission_Id" });
            DropIndex("dbo.BgcRolePermission", new[] { "BgcRole_Id" });
            DropIndex("dbo.MediaTypeInfoComposerArticles", new[] { "ComposerArticle_Id" });
            DropIndex("dbo.MediaTypeInfoComposerArticles", new[] { "MediaTypeInfo_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Invitations", new[] { "Sender_Id" });
            DropIndex("dbo.MediaTypeInfos", new[] { "StorageId" });
            DropIndex("dbo.ComposerNames", new[] { "Composer_Id" });
            DropIndex("dbo.ComposerNames", new[] { "FullName" });
            DropIndex("dbo.ComposerNames", new[] { "LastName" });
            DropIndex("dbo.ComposerNames", new[] { "FirstName" });
            DropIndex("dbo.ComposerArticles", new[] { "LocalizedName_Id" });
            DropIndex("dbo.ComposerArticles", new[] { "Composer_Id" });
            DropIndex("dbo.ComposerArticles", new[] { "StorageId" });
            DropTable("dbo.InvitationBgcRoles");
            DropTable("dbo.BgcUserSettings");
            DropTable("dbo.BgcRolePermission");
            DropTable("dbo.MediaTypeInfoComposerArticles");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Invitations");
            DropTable("dbo.MediaTypeInfos");
            DropTable("dbo.ComposerNames");
            DropTable("dbo.Composers");
            DropTable("dbo.ComposerArticles");
        }
    }
}
