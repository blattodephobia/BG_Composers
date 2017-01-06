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
        private static void UpdateOrAdd(RouteValueDictionary dict, string key, object value)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key] = value;
            }
        }

        protected virtual string GetDefaulAction(string controllerName)
        {
            return MVC.Public.Controllers.ContainsKey(controllerName ?? string.Empty)
                    ? (MVC.Public.Controllers[controllerName] as BgcControllerBase).DefaultActionName
                    : null;
        }

        protected RouteValueDictionary GetCompleteRoute(RouteValueDictionary route)
        {
            RouteValueDictionary completeRoute = null;
            if (string.IsNullOrEmpty(route["locale"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                UpdateOrAdd(completeRoute, "locale", "en-US");
            }

            if (string.IsNullOrEmpty(route["controller"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                UpdateOrAdd(completeRoute, "controller", MVC.Public.Main.Name);
            }

            if (string.IsNullOrEmpty(route["action"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                string actionName = GetDefaulAction(route["controller"]?.ToString());
                UpdateOrAdd(completeRoute, "action", actionName);
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