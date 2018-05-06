using BGC.Core;
using BGC.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace BGC.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ComposersDbContext>
    {
        private static void SeedPermissions(ComposersDbContext context)
        {
            DiscoveredTypes permissionDiscovery = TypeDiscovery.Discover(mode: TypeDiscoveryMode.Strict, consumingType: typeof(BgcRoleManager));
            IEnumerable<Permission> discoveredPermissions = from permission in permissionDiscovery.DiscoveredTypesInheritingFrom<Permission>()
                                                            where !permission.IsAbstract
                                                            select (Permission)Activator.CreateInstance(permission);
            HashSet<Permission> dbPermissions = new HashSet<Permission>(context.Permissions);
            foreach (Permission permission in discoveredPermissions)
            {
                if (!dbPermissions.Contains(permission))
                {
                    context.Permissions.Add(permission);
                }
            }
        }

        private static void SeedRoles(ComposersDbContext context, BgcRoleManager roleManager)
        {
            BgcRole editorRole = roleManager.FindByName(nameof(EditorRole));
            if (editorRole == null)
            {
                editorRole = new EditorRole();
                roleManager.Create(editorRole);
            }
            var editorPermissions = new Permission[] { new ArticleManagementPermission(), new GlossaryManagementPermission() };
            foreach (Permission editorPermission in editorPermissions)
            {
                if (!editorRole.Permissions.Contains(editorPermission))
                {
                    editorRole.Permissions.Add(editorPermission);
                }
            }

            BgcRole adminRole = roleManager.FindByName(nameof(AdministratorRole));
            if (adminRole == null)
            {
                adminRole = new BgcRole() { Name = nameof(AdministratorRole) };
                roleManager.Create(adminRole);
            }

            foreach (Permission permission in context.Permissions.ToList())
            {
                if (!adminRole.Permissions.Contains(permission))
                {
                    adminRole.Permissions.Add(permission);
                }
            }
        }

        protected override void Seed(ComposersDbContext context)
        {
            try
            {
                var roleManager = new BgcRoleManager(new RoleStore<BgcRole, long, BgcUserRole>(context));
                var userManager = new BgcUserManager(
                    new UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>(context),
                    context.GetRepository<BgcRole>(),
                    context.GetRepository<Invitation>());

                SeedPermissions(context);
                SeedRoles(context, roleManager);
                context.SaveChanges();

                if (userManager.FindByName(BgcUser.AdministratorUserName) == null)
                {
                    BgcUser admin = new BgcUser(BgcUser.AdministratorUserName);
                    userManager.Create(admin, "__8ja&7.s9/G");
                    userManager.AddToRole(admin.Id, nameof(AdministratorRole));
                }

                context.Settings.AddOrUpdate(setting => setting.Name,
                    new MultiCultureInfoSetting("SupportedLanguages")
                    {
                        Cultures = new[]
                        {
                            CultureInfo.GetCultureInfo("bg-BG"),
                            CultureInfo.GetCultureInfo("en-US"),
                            CultureInfo.GetCultureInfo("de-DE"),
                        }
                    });

#if DEBUG
                byte[] id = new byte[16];
                id[id.Length - 1] = 1;
                Composer pStupel = new Composer() { Id = new Guid(id) };

                ComposerName[] names = new[]
                {
                    new ComposerName("Petar Stupel", "en-US")
                    {
                        Composer = pStupel
                    },
                    new ComposerName("Петър Ступел", "bg-BG")
                    {
                        Composer = pStupel
                    },
                    new ComposerName("Петър Ступел", "de-DE")
                    {
                        Composer = pStupel
                    }
                };

                pStupel.LocalizedNames = names;
                pStupel.Articles = new HashSet<ComposerArticle>()
                {
                    new ComposerArticle()
                    {
                        Composer = pStupel,
                        LocalizedName = pStupel.LocalizedNames.ElementAt(0),
                        Language = pStupel.LocalizedNames.ElementAt(0).Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new ComposerArticle()
                    {
                        Composer = pStupel,
                        LocalizedName = pStupel.LocalizedNames.ElementAt(1),
                        Language = pStupel.LocalizedNames.ElementAt(1).Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                    },
                    new ComposerArticle()
                    {
                        Composer = pStupel,
                        LocalizedName = pStupel.LocalizedNames.ElementAt(2),
                        Language = pStupel.LocalizedNames.ElementAt(2).Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                    }
                };
                context.Composers.AddOrUpdate(composer => composer.Id, pStupel);
#endif
                context.SaveChanges();
            }
            catch (Exception e)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Launch();
                }
                else
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        public Configuration()
        {
            CodeGenerator = new MySqlCodeGenerator();
        }
    }
}
