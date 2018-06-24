namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComposerArticle",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 0),
                        StorageId = c.Guid(nullable: false),
                        Composer_Id = c.Guid(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composer", t => t.Composer_Id, cascadeDelete: true)
                .Index(t => t.StorageId)
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "dbo.Composer",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateOfBirth = c.DateTime(precision: 0),
                        DateOfDeath = c.DateTime(precision: 0),
                        Order = c.Int(nullable: false),
                        Profile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComposerName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Composer_Id = c.Guid(nullable: false),
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        FullName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composer", t => t.Composer_Id, cascadeDelete: true)
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "dbo.ComposerProfile",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Composer_Id = c.Guid(nullable: false),
                        ProfilePicture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Composer", t => t.Composer_Id, cascadeDelete: true)
                .ForeignKey("dbo.MediaTypeInfo", t => t.ProfilePicture_Id)
                .ForeignKey("dbo.Composer", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.Composer_Id)
                .Index(t => t.ProfilePicture_Id);
            
            CreateTable(
                "dbo.MediaTypeInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MimeType = c.String(unicode: false),
                        StorageId = c.Guid(nullable: false),
                        OriginalFileName = c.String(unicode: false),
                        ExternalLocation = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StorageId);
            
            CreateTable(
                "dbo.ComposerArticle_MediaTypeInfo",
                c => new
                    {
                        Article_Id = c.Long(nullable: false),
                        MediaEntry_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.MediaEntry_Id })
                .ForeignKey("dbo.ComposerArticle", t => t.Article_Id, name: "FK_Articles_Id", cascadeDelete: true)
                .ForeignKey("dbo.MediaTypeInfo", t => t.MediaEntry_Id, name: "FK_Media_Id", cascadeDelete: true)
                .Index(t => t.Article_Id)
                .Index(t => t.MediaEntry_Id);
            
            CreateTable(
                "dbo.GlossaryEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GlossaryDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false, unicode: false),
                        Term = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        Definition = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        GlossaryEntry_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GlossaryEntries", t => t.GlossaryEntry_Id, cascadeDelete: true)
                .Index(t => t.GlossaryEntry_Id);
            
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
                        UserName = c.String(nullable: false, maxLength: 32, storeType: "nvarchar"),
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
                        Name = c.String(nullable: false, unicode: false),
                        Description = c.String(maxLength: 1000, storeType: "nvarchar"),
                        StringValue = c.String(maxLength: 1000, storeType: "nvarchar"),
                        Priority = c.Int(nullable: false),
                        OwnerStamp = c.String(maxLength: 64, storeType: "nvarchar"),
                        Date = c.DateTime(precision: 0),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.GlossaryDefinitions", "GlossaryEntry_Id", "dbo.GlossaryEntries");
            DropForeignKey("dbo.ComposerArticle_MediaTypeInfo", "MediaEntry_Id", "dbo.MediaTypeInfo");
            DropForeignKey("dbo.ComposerArticle_MediaTypeInfo", "Article_Id", "dbo.ComposerArticle");
            DropForeignKey("dbo.ComposerArticle", "Composer_Id", "dbo.Composer");
            DropForeignKey("dbo.ComposerProfile", "Id", "dbo.Composer");
            DropForeignKey("dbo.ComposerProfile", "ProfilePicture_Id", "dbo.MediaTypeInfo");
            DropForeignKey("dbo.ComposerProfile", "Composer_Id", "dbo.Composer");
            DropForeignKey("dbo.ComposerName", "Composer_Id", "dbo.Composer");
            DropIndex("dbo.InvitationBgcRoles", new[] { "BgcRole_Id" });
            DropIndex("dbo.InvitationBgcRoles", new[] { "Invitation_Id" });
            DropIndex("dbo.BgcUserSettings", new[] { "Setting_Id" });
            DropIndex("dbo.BgcUserSettings", new[] { "BgcUser_Id" });
            DropIndex("dbo.BgcRolePermission", new[] { "Permission_Id" });
            DropIndex("dbo.BgcRolePermission", new[] { "BgcRole_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Invitations", new[] { "Sender_Id" });
            DropIndex("dbo.GlossaryDefinitions", new[] { "GlossaryEntry_Id" });
            DropIndex("dbo.ComposerArticle_MediaTypeInfo", new[] { "MediaEntry_Id" });
            DropIndex("dbo.ComposerArticle_MediaTypeInfo", new[] { "Article_Id" });
            DropIndex("dbo.MediaTypeInfo", new[] { "StorageId" });
            DropIndex("dbo.ComposerProfile", new[] { "ProfilePicture_Id" });
            DropIndex("dbo.ComposerProfile", new[] { "Composer_Id" });
            DropIndex("dbo.ComposerProfile", new[] { "Id" });
            DropIndex("dbo.ComposerName", new[] { "Composer_Id" });
            DropIndex("dbo.ComposerArticle", new[] { "Composer_Id" });
            DropIndex("dbo.ComposerArticle", new[] { "StorageId" });
            DropTable("dbo.InvitationBgcRoles");
            DropTable("dbo.BgcUserSettings");
            DropTable("dbo.BgcRolePermission");
            DropTable("dbo.Settings");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Permissions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Invitations");
            DropTable("dbo.GlossaryDefinitions");
            DropTable("dbo.GlossaryEntries");
            DropTable("dbo.ComposerArticle_MediaTypeInfo");
            DropTable("dbo.MediaTypeInfo");
            DropTable("dbo.ComposerProfile");
            DropTable("dbo.ComposerName");
            DropTable("dbo.Composer");
            DropTable("dbo.ComposerArticle");
        }
    }
}
