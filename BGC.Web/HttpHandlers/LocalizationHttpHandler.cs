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

        private static readonly string LocaleCookieName = LocaleRouteTokenName;
        private static readonly string LocaleRouteTokenName = "locale";
        private static readonly IEnumerable<CultureInfo> SupportedCultures;

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

        protected virtual string GetDefaultAction(string controllerName)
        {
            return MVC.Public.Controllers.ContainsKey(controllerName ?? string.Empty)
                    ? (MVC.Public.Controllers[controllerName] as BgcControllerBase).DefaultActionName
                    : null;
        }

        protected RouteValueDictionary GetCompleteRoute(RouteValueDictionary route)
        {
            RouteValueDictionary completeRoute = null;
            if (Locale.EffectiveValue.Name != route[LocaleRouteTokenName]?.ToString())
            {
                completeRoute = new RouteValueDictionary(route);
                UpdateOrAdd(completeRoute, LocaleRouteTokenName, Locale.EffectiveValue.Name);
            }

            if (string.IsNullOrEmpty(route["controller"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                UpdateOrAdd(completeRoute, "controller", MVC.Public.Main.Name);
            }

            if (string.IsNullOrEmpty(route["action"]?.ToString()))
            {
                completeRoute = completeRoute ?? new RouteValueDictionary(route);
                string actionName = GetDefaultAction(route["controller"]?.ToString());
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

        public RequestContextLocale Locale { get; private set; }

        protected LocalizationHttpHandler(RequestContext context, RequestContextLocale locale) :
            base(context)
        {
            Locale = locale;
        }

        public LocalizationHttpHandler(RequestContext context) :
            this(context, new MaxMindGeoLocationService(File.OpenRead(HttpRuntime.AppDomainAppPath + @"App_Data\Geolocation\GeoLite2-Country.mmdb")))
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