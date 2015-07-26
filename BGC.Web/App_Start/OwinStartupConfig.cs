using BGC.WebAPI.Areas.Administration;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.WebAPI.App_Start
{
	public class OwinStartupConfig
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString(string.Format(
					"/{0}/{1}/{2}",
					"admin",
					AdministrationAreaRegistration.UrlPrefixToken,
					MVC.AdministrationArea.Authentication.Name,
					MVC.AdministrationArea.Authentication.ActionNames.Login)),
				CookieName = "BGC.Auth"
			});
		}
	}
}