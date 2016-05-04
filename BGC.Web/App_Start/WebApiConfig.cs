using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;

namespace BGC.Web
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
            if (MVC.Debugging)
            {
                config.EnableSystemDiagnosticsTracing();
            }
		}
	}
}
