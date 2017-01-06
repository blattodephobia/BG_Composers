using BGC.Web.Controllers;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.HttpHandlers
{
    public class LocalizationHttpHandler : MvcHandler
    {
        protected RouteValueDictionary GetCompleteRoute(RouteValueDictionary route)
        {
            RouteValueDictionary completeRoute = null;
            if (string.IsNullOrEmpty(route["locale"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                completeRoute.Add("locale", "en-US");
            }

            if (string.IsNullOrEmpty(route["controller"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                completeRoute.Add("controller", MVC.Public.Main.Name);
            }

            if (string.IsNullOrEmpty(route["action"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                string actionName = MVC.Public.Controllers.ContainsKey(route["controller"]?.ToString() ?? string.Empty)
                    ? (MVC.Public.Controllers[route["controller"].ToString()] as BgcControllerBase).DefaultActionName
                    : null;
                completeRoute["action"] = actionName;
            }

            return completeRoute;
        }

        protected override IAsyncResult BeginProcessRequest(HttpContextBase httpContext, AsyncCallback callback, object state)
        {
            RouteValueDictionary validRouteTokens = GetCompleteRoute(RequestContext.RouteData.Values);
            if (validRouteTokens != null)
            {
                Action<RouteValueDictionary> redirectAction = httpContext.Response.RedirectToRoute;
                return redirectAction.BeginInvoke(validRouteTokens, callback, state);
            }
            else
            {
                return base.BeginProcessRequest(httpContext, callback, state);
            }
        }

        protected override void EndProcessRequest(IAsyncResult asyncResult)
        {
            if (!(asyncResult is System.Runtime.Remoting.Messaging.AsyncResult))
            {
                base.EndProcessRequest(asyncResult);
            }
        }

        protected override void ProcessRequest(HttpContext context)
        {
            RouteValueDictionary validRouteTokens = GetCompleteRoute(RequestContext.RouteData.Values);
            if (validRouteTokens != null)
            {
                context.Response.RedirectToRoute(validRouteTokens);
            }
            else
            {
                base.ProcessRequest(context);
            }
        }

        public LocalizationHttpHandler(RequestContext context) :
            base(context)
        {
        }
    }
}