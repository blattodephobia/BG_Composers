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
		public void Configuration(IAppBuilder app)
		{
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieName = "BGC.Auth"
            });
        }
	}
}