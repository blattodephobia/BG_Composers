using BGC.Core;
using BGC.Web.HttpHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.RouteHandlers
{
    public class LocalizationRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext) => new LocalizationHttpHandler(requestContext, DependencyResolver.Current.GetService<WebApplicationSettings>());
    }
}