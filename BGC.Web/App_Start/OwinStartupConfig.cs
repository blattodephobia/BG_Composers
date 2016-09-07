using BGC.Web.Areas.Administration;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.App_Start
{
	public class OwinStartupConfig
	{
        private static readonly TimeSpan CookieExpiration = TimeSpan.FromDays(30);

		public void Configuration(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions()
			{
                AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString(string.Format(
					"/{0}/{1}/{2}",
					AdministrationAreaRegistration.UrlPrefixToken,
					MVC.AdministrationArea.Authentication.Name,
					MVC.AdministrationArea.Authentication.ActionNames.Login)),
				CookieName = "BGC.Auth",
                ExpireTimeSpan = CookieExpiration,
                SlidingExpiration = true
            });
		}
	}
}