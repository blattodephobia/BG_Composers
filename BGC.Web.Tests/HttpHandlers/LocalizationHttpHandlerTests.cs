using BGC.Web.HttpHandlers;
using BGC.Web.Models;
using BGC.Web.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using TestUtils;
using static BGC.Web.WebApiApplication;

namespace BGC.Web.Tests.HttpHandlers
{
    public class LocalizationHttpHandlerProxy : LocalizationHttpHandler
    {
        public static HttpCookie GetLocaleCookie() => new HttpCookie(LocaleCookieName);

        public RouteValueDictionary GetCompleteRouteProxy(RouteValueDictionary route) => GetCompleteRoute(route);

        public string DefaultAction { get; set; } = "ACTION";

        public string LocaleRouteTokenNameProxy => LocaleRouteTokenName;

        protected override string GetDefaultAction(string controllerName) => DefaultAction;

        public RouteValueDictionary ProcessRouteProxy() => ProcessRoute();

        public LocalizationHttpHandlerProxy(RequestContext ctx, IEnumerable<CultureInfo> supportedCultures) :
            base(ctx, RequestContextLocale.FromCookie(supportedCultures, GetLocaleCookie()))
        {
        }

        public LocalizationHttpHandlerProxy(RequestContext ctx, RequestContextLocale locale) :
            base(ctx, locale)
        {

        }
    }

    [TestFixture]
    public class GetCompleteRouteTests
    {
        private readonly LocalizationHttpHandlerProxy _handler = new LocalizationHttpHandlerProxy(new RequestContext(), new List<CultureInfo>() { CultureInfo.GetCultureInfo("en-US") });

        [Test]
        public void PopulatesMissingLocaleToken_KeyMissing()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "controller", "c" },
                { "action", "c" },
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("locale"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["locale"]?.ToString()));
        }

        [Test]
        public void PopulatesMissingLocaleToken_ValueEmpty()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "controller", "c" },
                { "action", "c" },
                { "locale", "" }
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("locale"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["locale"]?.ToString()));
        }

        [Test]
        public void PopulatesMissingControllerToken_KeyMissing()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "locale", "c" },
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("controller"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["controller"]?.ToString()));
        }

        [Test]
        public void PopulatesMissingControllerToken_ValueEmpty()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "locale", "en-US" }
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("controller"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["controller"]?.ToString()));
        }

        [Test]
        public void PopulatesMissingActionToken_KeyMissing()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "locale", "en-US" },
                { "controller", "c" },
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("action"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["action"]?.ToString()));
        }

        [Test]
        public void PopulatesMissingActionToken_ValueEmpty()
        {
            RouteValueDictionary dict = new RouteValueDictionary()
            {
                { "locale", "en-US" },
                { "controller", "c" },
            };
            RouteValueDictionary completeRoute = _handler.GetCompleteRouteProxy(dict);
            Assert.IsNotNull(completeRoute);
            Assert.IsTrue(completeRoute.ContainsKey("action"));
            Assert.IsFalse(string.IsNullOrEmpty(completeRoute["action"]?.ToString()));
        }
    }

    [TestFixture]
    public class LocaleCookieTests
    {
        [Test]
        public void SetsCookieWithLastUsedLocale()
        {
            var req = new RequestContext() { RouteData = new RouteData() };
            req.RouteData.Values.Add(LocaleRouteTokenName, "de-DE");
            var supportedLocales = new[] { CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE") };

            HttpCookie localeCookie = new HttpCookie(LocaleCookieName);
            RequestContextLocale reqLocale = RequestContextLocale.FromCookie(supportedLocales, localeCookie);
            reqLocale.ValidRouteLocale.SetValue(CultureInfo.GetCultureInfo(req.RouteData.Values[LocaleRouteTokenName] as string));

            LocalizationHttpHandlerProxy handler = new LocalizationHttpHandlerProxy(req, reqLocale);
            handler.ProcessRouteProxy();

            Assert.AreEqual("de-DE", localeCookie.Values[handler.LocaleRouteTokenNameProxy]);
        }
    }
}
