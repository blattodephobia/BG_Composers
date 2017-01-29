using BGC.Utilities;
using RouteDebug;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BGC.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		public class TempDataKeys
		{
			public class AdministrationArea
			{
				public static readonly string LoginSuccessReturnUrl = nameof(LoginSuccessReturnUrl);
			}
		}

		public class ASF<T> : IComparable<T>
		{
			public int CompareTo(T other)
			{
				return 0;
			}
        }

        public static readonly string LocaleRouteTokenName = "locale";
        public static readonly string LocaleCookieName = LocaleRouteTokenName;

        public static readonly IEnumerable<CultureInfo> SupportedCultures = new CultureInfo[]
        {
            CultureInfo.GetCultureInfo("en-US"),
            CultureInfo.GetCultureInfo("bg-BG"),
            CultureInfo.GetCultureInfo("de-DE"),
        };

		protected void Application_Start()
		{
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AreaRegistration.RegisterAllAreas();
		}
	}
}