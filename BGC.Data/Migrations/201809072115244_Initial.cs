namespace BGC.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ComposerArticle_MediaTypeInfo",
                c => new
                    {
                        Article_Id = c.Long(nullable: false),
                        MediaEntry_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Article_Id, t.MediaEntry_Id })
                .ForeignKey("ComposerArticle", t => t.Article_Id, cascadeDelete: true, name: "FK_888D210D:ComposerArticle_MediaTypeInfo.Article_Id")
                .ForeignKey("MediaTypeInfo", t => t.MediaEntry_Id, cascadeDelete: true, name: "FK_942D5AEA:ComposerArticle_MediaTypeInfo.MediaEntry_Id")
                .Index(t => t.Article_Id)
                .Index(t => t.MediaEntry_Id);
            
            CreateTable(
                "ComposerArticle",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StorageId = c.Guid(nullable: false),
                        Composer_Id = c.Guid(nullable: false),
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        CreatedUtc = c.DateTime(nullable: false, precision: 0),
                        IsArchived = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Composer", t => t.Composer_Id, cascadeDelete: true, name: "FK_24F084A:ComposerArticle.Composer_Id")
                .Index(t => t.StorageId)
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "Composer",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DateAdded = c.DateTime(nullable: false, precision: 0),
                        DateOfBirth = c.DateTime(precision: 0),
                        DateOfDeath = c.DateTime(precision: 0),
                        Order = c.Int(nullable: false),
                        ProfilePicture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("MediaTypeInfo", t => t.ProfilePicture_Id, name: "FK_942D5AEA:Composer.ProfilePicture_Id")
                .Index(t => t.ProfilePicture_Id);
            
            CreateTable(
                "ComposerName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Composer_Id = c.Guid(nullable: false),
                        Language = c.String(nullable: false, maxLength: 5, storeType: "nvarchar"),
                        FullName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Composer", t => t.Composer_Id, cascadeDelete: true, name: "FK_24F084A:ComposerName.Composer_Id")
                .Index(t => t.Composer_Id);
            
            CreateTable(
                "ComposerMediaRelationalDtoes",
                c => new
                    {
                        Composer_Id = c.Guid(nullable: false),
                        MediaTypeInfo_Id = c.Int(nullable: false),
                        Purpose = c.String(unicode: false),
                    })
                .PrimaryKey(t => new { t.Composer_Id, t.MediaTypeInfo_Id })
                .ForeignKey("Composer", t => t.Composer_Id, cascadeDelete: true, name: "FK_24F084A:ComposerMediaRelationalDtoes.Composer_Id")
                .ForeignKey("MediaTypeInfo", t => t.MediaTypeInfo_Id, cascadeDelete: true, name: "FK_942D5AEA:ComposerMediaRelationalDtoes.MediaTypeInfo_Id")
                .Index(t => t.Composer_Id)
                .Index(t => t.MediaTypeInfo_Id);
            
            CreateTable(
                "MediaTypeInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MimeType = c.String(nullable: false, unicode: false),
                        StorageId = c.Guid(nullable: false),
                        OriginalFileName = c.String(unicode: false),
                        ExternalLocation = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.StorageId);
            
            CreateTable(
                "GlossaryEntries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "GlossaryDefinitions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Language = c.String(nullable: false, unicode: false),
                        Term = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        Definition = c.String(nullable: false, maxLength: 1000, storeType: "nvarchar"),
                        GlossaryEntry_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("GlossaryEntries", t => t.GlossaryEntry_Id, cascadeDelete: true, name: "FK_29564CE6:GlossaryDefinitions.GlossaryEntry_Id")
                .Index(t => t.GlossaryEntry_Id);
            
            CreateTable(
                "Invitations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Email = c.String(nullable: false, unicode: false),
                        ExpirationDate = c.DateTime(nullable: false, precision: 0),
                        IsObsolete = c.Boolean(nullable: false),
                        Sender_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("AspNetUsers", t => t.Sender_Id, cascadeDelete: true, name: "FK_62BA4819:Invitations.Sender_Id")
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "AspNetRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "AspNetUserRoles",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("AspNetRoles", t => t.RoleId, cascadeDelete: true, name: "FK_A238FC95:AspNetUserRoles.RoleId")
                .ForeignKey("AspNetUsers", t => t.UserId, cascadeDelete: true, name: "FK_62BA4819:AspNetUserRoles.UserId")
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "AspNetUsers",
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
                "AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ClaimType = c.String(unicode: false),
                        ClaimValue = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("AspNetUsers", t => t.UserId, cascadeDelete: true, name: "FK_62BA4819:AspNetUserClaims.UserId")
                .Index(t => t.UserId);
            
            CreateTable(
                "AspNetUserLogins",
                c => new
                    {
                        UserId = c.Long(nullable: false),
                        ProviderKey = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        LoginProvider = c.String(unicode: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.ProviderKey })
                .ForeignKey("AspNetUsers", t => t.UserId, cascadeDelete: true, name: "FK_62BA4819:AspNetUserLogins.UserId")
                .Index(t => t.UserId);
            
            CreateTable(
                "Settings",
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
                "BgcRolePermission",
                c => new
                    {
                        BgcRole_Id = c.Long(nullable: false),
                        Permission_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.BgcRole_Id, t.Permission_Id })
                .ForeignKey("AspNetRoles", t => t.BgcRole_Id, cascadeDelete: true, name: "FK_A238FC95:BgcRolePermission.BgcRole_Id")
                .ForeignKey("Permissions", t => t.Permission_Id, cascadeDelete: true, name: "FK_881C002F:BgcRolePermission.Permission_Id")
                .Index(t => t.BgcRole_Id)
                .Index(t => t.Permission_Id);
            
            CreateTable(
                "BgcUserSettings",
                c => new
                    {
                        BgcUser_Id = c.Long(nullable: false),
                        Setting_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.BgcUser_Id, t.Setting_Id })
                .ForeignKey("AspNetUsers", t => t.BgcUser_Id, cascadeDelete: true, name: "FK_62BA4819:BgcUserSettings.BgcUser_Id")
                .ForeignKey("Settings", t => t.Setting_Id, cascadeDelete: true, name: "FK_BC82793:BgcUserSettings.Setting_Id")
                .Index(t => t.BgcUser_Id)
                .Index(t => t.Setting_Id);
            
            CreateTable(
                "InvitationBgcRoles",
                c => new
                    {
                        Invitation_Id = c.Guid(nullable: false),
                        BgcRole_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Invitation_Id, t.BgcRole_Id })
                .ForeignKey("Invitations", t => t.Invitation_Id, cascadeDelete: true, name: "FK_6FCF3C4:InvitationBgcRoles.Invitation_Id")
                .ForeignKey("AspNetRoles", t => t.BgcRole_Id, cascadeDelete: true, name: "FK_A238FC95:InvitationBgcRoles.BgcRole_Id")
                .Index(t => t.Invitation_Id)
                .Index(t => t.BgcRole_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Invitations", "FK_62BA4819:Invitations.Sender_Id");
            DropForeignKey("InvitationBgcRoles", "FK_A238FC95:InvitationBgcRoles.BgcRole_Id");
            DropForeignKey("InvitationBgcRoles", "FK_6FCF3C4:InvitationBgcRoles.Invitation_Id");
            DropForeignKey("BgcUserSettings", "FK_BC82793:BgcUserSettings.Setting_Id");
            DropForeignKey("BgcUserSettings", "FK_62BA4819:BgcUserSettings.BgcUser_Id");
            DropForeignKey("AspNetUserRoles", "FK_62BA4819:AspNetUserRoles.UserId");
            DropForeignKey("AspNetUserLogins", "FK_62BA4819:AspNetUserLogins.UserId");
            DropForeignKey("AspNetUserClaims", "FK_62BA4819:AspNetUserClaims.UserId");
            DropForeignKey("AspNetUserRoles", "FK_A238FC95:AspNetUserRoles.RoleId");
            DropForeignKey("BgcRolePermission", "FK_881C002F:BgcRolePermission.Permission_Id");
            DropForeignKey("BgcRolePermission", "FK_A238FC95:BgcRolePermission.BgcRole_Id");
            DropForeignKey("GlossaryDefinitions", "FK_29564CE6:GlossaryDefinitions.GlossaryEntry_Id");
            DropForeignKey("ComposerArticle_MediaTypeInfo", "FK_942D5AEA:ComposerArticle_MediaTypeInfo.MediaEntry_Id");
            DropForeignKey("ComposerArticle_MediaTypeInfo", "FK_888D210D:ComposerArticle_MediaTypeInfo.Article_Id");
            DropForeignKey("ComposerArticle", "FK_24F084A:ComposerArticle.Composer_Id");
            DropForeignKey("Composer", "FK_942D5AEA:Composer.ProfilePicture_Id");
            DropForeignKey("ComposerMediaRelationalDtoes", "FK_942D5AEA:ComposerMediaRelationalDtoes.MediaTypeInfo_Id");
            DropForeignKey("ComposerMediaRelationalDtoes", "FK_24F084A:ComposerMediaRelationalDtoes.Composer_Id");
            DropForeignKey("ComposerName", "FK_24F084A:ComposerName.Composer_Id");
            DropIndex("InvitationBgcRoles", new[] { "BgcRole_Id" });
            DropIndex("InvitationBgcRoles", new[] { "Invitation_Id" });
            DropIndex("BgcUserSettings", new[] { "Setting_Id" });
            DropIndex("BgcUserSettings", new[] { "BgcUser_Id" });
            DropIndex("BgcRolePermission", new[] { "Permission_Id" });
            DropIndex("BgcRolePermission", new[] { "BgcRole_Id" });
            DropIndex("AspNetUserLogins", new[] { "UserId" });
            DropIndex("AspNetUserClaims", new[] { "UserId" });
            DropIndex("AspNetUsers", "UserNameIndex");
            DropIndex("AspNetUserRoles", new[] { "RoleId" });
            DropIndex("AspNetUserRoles", new[] { "UserId" });
            DropIndex("AspNetRoles", "RoleNameIndex");
            DropIndex("Invitations", new[] { "Sender_Id" });
            DropIndex("GlossaryDefinitions", new[] { "GlossaryEntry_Id" });
            DropIndex("MediaTypeInfo", new[] { "StorageId" });
            DropIndex("ComposerMediaRelationalDtoes", new[] { "MediaTypeInfo_Id" });
            DropIndex("ComposerMediaRelationalDtoes", new[] { "Composer_Id" });
            DropIndex("ComposerName", new[] { "Composer_Id" });
            DropIndex("Composer", new[] { "ProfilePicture_Id" });
            DropIndex("ComposerArticle", new[] { "Composer_Id" });
            DropIndex("ComposerArticle", new[] { "StorageId" });
            DropIndex("ComposerArticle_MediaTypeInfo", new[] { "MediaEntry_Id" });
            DropIndex("ComposerArticle_MediaTypeInfo", new[] { "Article_Id" });
            DropTable("InvitationBgcRoles");
            DropTable("BgcUserSettings");
            DropTable("BgcRolePermission");
            DropTable("Settings");
            DropTable("AspNetUserLogins");
            DropTable("AspNetUserClaims");
            DropTable("AspNetUsers");
            DropTable("AspNetUserRoles");
            DropTable("Permissions");
            DropTable("AspNetRoles");
            DropTable("Invitations");
            DropTable("GlossaryDefinitions");
            DropTable("GlossaryEntries");
            DropTable("MediaTypeInfo");
            DropTable("ComposerMediaRelationalDtoes");
            DropTable("ComposerName");
            DropTable("Composer");
            DropTable("ComposerArticle");
            DropTable("ComposerArticle_MediaTypeInfo");
        }
    }
}
