// <auto-generated />
namespace BGC.Data.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.2-31219")]
    public sealed partial class BgcUser_PasswordResetTokenHash : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(BgcUser_PasswordResetTokenHash));
        
        string IMigrationMetadata.Id
        {
            get { return "201611061323408_BgcUser_PasswordResetTokenHash"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
