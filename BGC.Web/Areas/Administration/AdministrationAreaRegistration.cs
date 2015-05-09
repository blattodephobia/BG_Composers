using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration
{
	public class AdministrationAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.AdministrationArea.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.Routes
				.MapRoute(
					name: "Administration",
					url: "admin/{controller}/{action}",
					defaults: new { controller = MVC.AdministrationArea.Account.Name, action = MVC.AdministrationArea.Account.ActionNames.Users })
				.DataTokens.Add("area", this.AreaName);
		}
	}
}