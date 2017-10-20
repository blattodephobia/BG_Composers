using BGC.Core;
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
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.HttpHandlers
{
    public class LocalizationHttpHandlerProxy : LocalizationHttpHandler
    {
        public RouteValueDictionary GetCompleteRouteProxy(RouteValueDictionary route) => GetCompleteRoute(route);

        public string DefaultAction { get; set; } = "ACTION";

        protected override string GetDefaultAction(string controllerName) => DefaultAction;

        public RouteValueDictionary ProcessRouteProxy() => ProcessRoute();

        public LocalizationHttpHandlerProxy(RequestContext ctx, WebApplicationSettings appProfile) :
            this(ctx, new RequestContextLocale(appProfile, new HttpCookie(appProfile.LocaleCookieName)))
        {
        }

        public LocalizationHttpHandlerProxy(RequestContext ctx, RequestContextLocale locale) :
            base(ctx, locale, GetStandardAppProfile().LocaleRouteTokenName)
        {
        }
    }

    [TestFixture]
    public class GetCompleteRouteTests
    {
        private readonly LocalizationHttpHandlerProxy _handler = new LocalizationHttpHandlerProxy(
            ctx: new RequestContext(),
            appProfile: GetStandardAppProfile());

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
    public class CtorTests
    {
        [Test]
        public void ChecksAppProfileForValidRouteTokenName_1()
        {
            Assert.Throws<InvalidOperationException>(() => new LocalizationHttpHandler(
                context: new RequestContext()
                {
                    HttpContext = GetMockHttpContextBase(GetMockRequestBase().Object, GetMockResponseBase().Object).Object
                },
                svc: GetMockGeoLocationService().Object,
                appProfile: new WebApplicationSettings()));
        }

        [Test]
        public void ChecksAppProfileForValidRouteTokenName_2()
        {
            Assert.Throws<InvalidOperationException>(() => new LocalizationHttpHandler(
                context: new RequestContext()
                {
                    HttpContext = GetMockHttpContextBase(GetMockRequestBase().Object, GetMockResponseBase().Object).Object
                },
                svc: GetMockGeoLocationService().Object,
                appProfile: new WebApplicationSettings() { LocaleRouteTokenName = "" }));
        }
    }

    [TestFixture]
    public class LocaleCookieTests
    {
        [Test]
        public void SetsCookieWithLastUsedLocale()
        {
            WebApplicationSettings testProfile = GetStandardAppProfile();
            testProfile.Seal();

            var req = new RequestContext() { RouteData = new RouteData() };
            req.RouteData.Values.Add(testProfile.LocaleRouteTokenName, "de-DE");

            HttpCookie localeCookie = new HttpCookie(testProfile.LocaleCookieName);
            RequestContextLocale reqLocale = new RequestContextLocale(testProfile, localeCookie);
            reqLocale.SetValidLocaleOrDefault(reqLocale.ValidRouteLocale, req.RouteData.Values[testProfile.LocaleRouteTokenName] as string);

            LocalizationHttpHandlerProxy handler = new LocalizationHttpHandlerProxy(req, reqLocale);
            handler.ProcessRouteProxy();

            Assert.AreEqual("de-DE", localeCookie.Values[testProfile.LocaleKey]);
        }
    }
}
