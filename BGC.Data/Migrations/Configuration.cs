namespace BGC.Data.Migrations
{
	using BGC.Data.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

	internal sealed class Configuration : DbMigrationsConfiguration<BGC.Data.ComposersDbContext>
	{
		protected override void Seed(BGC.Data.ComposersDbContext context)
		{
			try
			{
				var roleManager = new RoleManager<AspNetRole, long>(new RoleStore<AspNetRole, long, AspNetUserRole>(context));
				var userManager = new UserManager<AspNetUser, long>(new UserStore<AspNetUser, AspNetRole, long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>(context));

				if (!roleManager.RoleExists(AspNetRole.AdministratorRoleName))
				{
					roleManager.Create(new AspNetRole(AspNetRole.AdministratorRoleName));
				}

				if (userManager.FindByName(AspNetUser.AdministratorUserName) == null)
				{
					AspNetUser admin = new AspNetUser(AspNetUser.AdministratorUserName);
					userManager.Create(admin, "__8ja&7.s9/G");
					userManager.AddToRole(admin.Id, AspNetRole.AdministratorRoleName);
				}
			}
			catch (Exception ex)
			{
				if (!System.Diagnostics.Debugger.IsAttached)
				{
					System.Diagnostics.Debugger.Launch();
				}
			}
		}
	}
}
