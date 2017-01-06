using BGC.Web.RouteHandlers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.Areas.Public
{
	public class PublicAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.Public.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            Route standardRoute = new Route("{locale}/{controller}/{action}", new LocalizationRouteHandler())
            {
                Defaults = new RouteValueDictionary() { { "controller", MVC.Public.Main.Name }, { "action", UrlParameter.Optional } },
                DataTokens = new RouteValueDictionary() { { "area", AreaName } }
            };

            context
                .Routes
                .Add(standardRoute);

            Route incompleteRoute = new Route("", new LocalizationRouteHandler());
            incompleteRoute.DataTokens = new RouteValueDictionary(new { area = AreaName });
            context
                .Routes
                .Add(incompleteRoute);

            context.Routes.AppendTrailingSlash = true;
        }
	}
}