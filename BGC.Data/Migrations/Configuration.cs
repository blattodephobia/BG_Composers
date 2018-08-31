using BGC.Core;
using BGC.Data.Relational;
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
                            "bg-BG".ToCultureInfo(),
                            "en-US".ToCultureInfo(),
                            "de-DE".ToCultureInfo(),
                        }
                    });

#if DEBUG
                byte[] id = new byte[16];
                id[id.Length - 1] = 1;
                ComposerRelationalDto pStupel = new ComposerRelationalDto() { Id = new Guid(id) };

                NameRelationalDto[] names = new[]
                {
                    new NameRelationalDto
                    {
                        FullName = "Petar Stupel",
                        Language = "en-US",
                        Composer = pStupel
                    },
                    new NameRelationalDto()
                    {
                        FullName = "Петър Ступел",
                        Language = "bg-BG",
                        Composer = pStupel
                    },
                    new NameRelationalDto()
                    {
                        FullName = "Petar Stupel",
                        Language = "de-DE",
                        Composer = pStupel
                    }
                };
                
                var articles = new HashSet<ArticleRelationalDto>()
                {
                    new ArticleRelationalDto()
                    {
                        Composer = pStupel,
                        Language = names[0].Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000001")
                    },
                    new ArticleRelationalDto()
                    {
                        Composer = pStupel,
                        Language = names[1].Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000002")
                    },
                    new ArticleRelationalDto()
                    {
                        Composer = pStupel,
                        Language = names[2].Language,
                        StorageId = Guid.Parse("00000000-0000-0000-0000-000000000003")
                    }
                };
                pStupel.LocalizedNames.AddRange(names);
                pStupel.Articles.AddRange(articles);
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
