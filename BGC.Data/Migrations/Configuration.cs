using BGC.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity.Migrations;

namespace BGC.Data.Migrations
{
	internal sealed class Configuration : DbMigrationsConfiguration<BGC.Data.ComposersDbContext>
	{
		protected override void Seed(BGC.Data.ComposersDbContext context)
		{
			try
			{
				var roleManager = new RoleManager<BgcRole, long>(new RoleStore<BgcRole, long, BgcUserRole>(context));
				var userManager = new UserManager<BgcUser, long>(new UserStore<BgcUser, BgcRole, long, BgcUserLogin, BgcUserRole, BgcUserClaim>(context));

				if (!roleManager.RoleExists(BgcRole.AdministratorRoleName))
				{
					roleManager.Create(new BgcRole(BgcRole.AdministratorRoleName));
				}

				if (userManager.FindByName(BgcUser.AdministratorUserName) == null)
				{
					BgcUser admin = new BgcUser(BgcUser.AdministratorUserName);
					userManager.Create(admin, "__8ja&7.s9/G");
					userManager.AddToRole(admin.Id, BgcRole.AdministratorRoleName);
				}
			}
			catch
			{
				if (!System.Diagnostics.Debugger.IsAttached)
				{
					System.Diagnostics.Debugger.Launch();
				}
			}
		}
	}
}
