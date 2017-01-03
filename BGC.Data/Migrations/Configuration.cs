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
            context.SaveChanges();
        }

        private static void SeedRoles(ComposersDbContext context, BgcRoleManager roleManager)
        {
            if (!roleManager.RoleExists(nameof(EditorRole)))
            {
                roleManager.Create(new EditorRole());
            }

            BgcRole adminRole = roleManager.FindByName(nameof(AdministratorRole));
            if (adminRole == null)
            {
                adminRole = new BgcRole() { Name = nameof(AdministratorRole) };
                roleManager.Create(adminRole);
            }
            adminRole.Permissions = context.Permissions.ToList();
            context.SaveChanges();
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
                    new CultureSupportSetting()
                    {
                        Name = "SupportedLanguages",
                        SupportedCultures = new[]
                        {
                            CultureInfo.GetCultureInfo("bg-BG"),
                            CultureInfo.GetCultureInfo("en-US"),
                        }
                    });

#if DEBUG
                Composer pStupel = new Composer() { Id = 1 };

                ComposerName[] names = new[]
                {
                    new ComposerName("Petar Stupel", "en-US")
                    {
                        Composer = pStupel
                    },
                    new ComposerName("Петър Ступел", "bg-BG")
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
                        LocalizedName = pStupel.LocalizedNames.First(),
                        Language = pStupel.LocalizedNames.First().Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new ComposerArticle()
                    {
                        Composer = pStupel,
                        LocalizedName = pStupel.LocalizedNames.Last(),
                        Language = pStupel.LocalizedNames.Last().Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000002")
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

                throw e;
            }
        }
    }
}
