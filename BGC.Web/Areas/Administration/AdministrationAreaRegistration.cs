using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration
{
	public class AdministrationAreaRegistration : AreaRegistration
	{
		public static readonly string UrlPrefixToken = "admin";

        public override string AreaName => MVC.AdministrationArea.Name;

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context.Routes
                .MapRoute(
                    name: $"{MVC.AdministrationArea.Name}1",
                    url: $"{UrlPrefixToken}/{{controller}}/{{action}}",
                    defaults: new { controller = MVC.AdministrationArea.Account.Name, action = MVC.AdministrationArea.Account.ActionNames.Users })
                .DataTokens.Add("area", AreaName);

            context.Routes
                .MapRoute(
					name: $"{MVC.AdministrationArea.Name}2",
					url: $"{MVC.AdministrationArea.Name}/{{controller}}/{{action}}",
					defaults: new { controller = MVC.AdministrationArea.Account.Name, action = MVC.AdministrationArea.Account.ActionNames.Users })
				.DataTokens.Add("area", AreaName);
		}
	}
}