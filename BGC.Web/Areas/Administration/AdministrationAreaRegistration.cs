using BGC.Web.Areas.Administration.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration
{
	public class AdministrationAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.AdministrationArea.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context
                .Routes
                .MapRoute(
					name: $"{MVC.AdministrationArea.Name}",
					url: $"{MVC.AdministrationArea.Name}" + "/{controller}/{action}",
					defaults: new
                    {
                        controller = nameof(AccountController),
                        action = MVC.AdministrationArea.Account.ActionNames.Activities
                    })
				.DataTokens.Add("area", AreaName);
		}
	}
}