using BGC.Core;
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
using static BGC.Web.WebApiApplication;

namespace BGC.Web.HttpHandlers
{
    public partial class LocalizationHttpHandler : MvcHandler
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
        
        private static void UpdateOrAdd(HttpCookieCollection cookies, string cookieName, TimeSpan expiration = default(TimeSpan))
        {

        }
        
        private string _localeRouteTokenName;

        protected virtual string GetDefaultAction(string controllerName)
        {
            return MVC.Public.Controllers.ContainsKey(controllerName ?? string.Empty)
                    ? (MVC.Public.Controllers[controllerName] as BgcControllerBase).DefaultActionName
                    : null;
        }

        protected RouteValueDictionary GetCompleteRoute(RouteValueDictionary route)
        {
            RouteValueDictionary completeRoute = null;

            if (Locale.EffectiveValue.Name != route?[_localeRouteTokenName]?.ToString())
            {
                completeRoute = route != null
                    ? new RouteValueDictionary(route)
                    : new RouteValueDictionary();
                UpdateOrAdd(completeRoute, _localeRouteTokenName, Locale.EffectiveValue.Name);
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
            RouteValueDictionary validRouteTokens = ProcessRoute();
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

        protected RouteValueDictionary ProcessRoute()
        {
            RouteValueDictionary validRouteTokens = GetCompleteRoute(RequestContext.RouteData?.Values);
            Locale.CookieLocale.SetValue(Locale.EffectiveValue);
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
            RouteValueDictionary validRouteTokens = ProcessRoute();
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

        protected LocalizationHttpHandler(RequestContext context, RequestContextLocale locale, string localeRouteTokenName) :
            base(context)
        {
            Shield.ArgumentNotNull(locale).ThrowOnError();
            Shield.IsNotNullOrEmpty(localeRouteTokenName);

            Locale = locale;
            _localeRouteTokenName = localeRouteTokenName;
        }

        public LocalizationHttpHandler(RequestContext context, ApplicationProfile appProfile) :
            this(context, DependencyResolver.Current.GetService<IGeoLocationService>(), appProfile)
        {
            Shield.ArgumentNotNull(context).ThrowOnError();
            Shield.ArgumentNotNull(appProfile).ThrowOnError();
            Shield.IsNotNullOrEmpty(appProfile.LocaleRouteTokenName).ThrowOnError();
        }

        public LocalizationHttpHandler(RequestContext context, IGeoLocationService svc, ApplicationProfile appProfile) :
            this(
                context: context,
                locale: RequestContextLocale.FromRequest(
                    appProfile: appProfile.ArgumentNotNull().GetValueOrThrow(),
                    geoLocationService: svc.ArgumentNotNull().GetValueOrThrow(),
                    request: context.ArgumentNotNull().GetValueOrThrow().HttpContext.Request,
                    cookie: context.HttpContext.Response.Cookies[appProfile.LocaleCookieName.IsNotNullOrEmpty().GetValueOrThrow()]),
                localeRouteTokenName: appProfile.ArgumentNotNull().GetValueOrThrow().LocaleRouteTokenName)
        {
            Shield.IsNotNullOrEmpty(appProfile.LocaleRouteTokenName).ThrowOnError();
        }
    }
}