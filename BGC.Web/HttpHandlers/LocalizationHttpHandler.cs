using BGC.Utilities;
using BGC.Web.Controllers;
using BGC.Web.Models;
using BGC.Web.Services;
using CodeShield;
using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.HttpHandlers
{
    public partial class LocalizationHttpHandler : MvcHandler
    {
        static LocalizationHttpHandler()
        {
            SupportedCultures = new[]
            {
                CultureInfo.GetCultureInfo("en-US"),
                CultureInfo.GetCultureInfo("bg-BG"),
                CultureInfo.GetCultureInfo("de-DE"),
            };
        }

        private static readonly IEnumerable<CultureInfo> SupportedCultures;
        protected static readonly string LocaleRouteTokenName = "locale";
        protected static readonly string LocaleCookieName = LocaleRouteTokenName;

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
        
        private static void UpdateOrAdd(HttpCookieCollection cookies, string cookieName, TimeSpan expiration = default(TimeSpan))
        {

        }

        protected virtual string GetDefaultAction(string controllerName)
        {
            return MVC.Public.Controllers.ContainsKey(controllerName ?? string.Empty)
                    ? (MVC.Public.Controllers[controllerName] as BgcControllerBase).DefaultActionName
                    : null;
        }

        protected RouteValueDictionary GetCompleteRoute(RouteValueDictionary route)
        {
            RouteValueDictionary completeRoute = null;
            if (Locale.EffectiveValue.Name != route?[LocaleRouteTokenName]?.ToString())
            {
                completeRoute = route != null
                    ? new RouteValueDictionary(route)
                    : new RouteValueDictionary();
                UpdateOrAdd(completeRoute, LocaleRouteTokenName, Locale.EffectiveValue.Name);
            }

            if (string.IsNullOrEmpty(route?["controller"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                UpdateOrAdd(completeRoute, "controller", MVC.Public.Main.Name);
            }

            if (string.IsNullOrEmpty(route?["action"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                string actionName = GetDefaultAction(route?["controller"]?.ToString());
                UpdateOrAdd(completeRoute, "action", actionName);
            }

            return completeRoute;
        }

        protected override IAsyncResult BeginProcessRequest(HttpContextBase httpContext, AsyncCallback callback, object state)
        {
            RouteValueDictionary validRouteTokens = ProcessRoute(httpContext.Response.Cookies.InitGet(LocaleCookieName));
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

        protected RouteValueDictionary ProcessRoute(HttpCookie outputCookie)
        {
            RouteValueDictionary validRouteTokens = GetCompleteRoute(RequestContext.RouteData?.Values);
            string locale = (validRouteTokens ?? RequestContext.RouteData.Values)["locale"] as string;
            outputCookie.Values["locale"] = locale;
            return validRouteTokens;
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
            RouteValueDictionary validRouteTokens = ProcessRoute(context.Request.Cookies.InitGet(LocaleCookieName));
            if (validRouteTokens != null)
            {
                context.Response.RedirectToRoute(validRouteTokens);
            }
            else
            {
                base.ProcessRequest(context);
            }
        }

        public RequestContextLocale Locale { get; private set; }

        protected LocalizationHttpHandler(RequestContext context, RequestContextLocale locale) :
            base(context)
        {
            Locale = locale;
        }

        public LocalizationHttpHandler(RequestContext context) :
            this(context, DependencyResolver.Current.GetService<IGeoLocationService>())
        {
        }

        public LocalizationHttpHandler(RequestContext context, IGeoLocationService svc) :
            this(
                context: context,
                locale: new RequestContextLocale(
                    request: context.ArgumentNotNull().GetValueOrThrow().HttpContext.Request,
                    supportedCultures: SupportedCultures,
                    geoLocationService: svc.ArgumentNotNull().GetValueOrThrow()))
        {
        }
    }
}