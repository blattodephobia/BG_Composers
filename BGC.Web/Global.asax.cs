using RouteDebug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BGC.WebAPI
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AreaRegistration.RegisterAllAreas();

			// RegisterRouteDebugger();
		}

		private static void RegisterRouteDebugger()
		{
			using (RouteTable.Routes.GetReadLock())
			{
				bool foundDebugRoute = false;
				foreach (Route route in RouteTable.Routes.OfType<Route>())
				{
					route.RouteHandler = new DebugRouteHandler();
					if (route == DebugRoute.Singleton)
						foundDebugRoute = true;
				}
				if (!foundDebugRoute)
				{
					RouteTable.Routes.Add(DebugRoute.Singleton);
				}
			}
		}
	}
}