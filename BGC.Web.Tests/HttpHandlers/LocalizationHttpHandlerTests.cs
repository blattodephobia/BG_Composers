using BGC.Web.HttpHandlers;
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

namespace BGC.Web.Tests.HttpHandlers
{
    public class RequestContextLocaleProxy : LocalizationHttpHandler.RequestContextLocale
    {
        public RequestContextLocaleProxy(List<CultureInfo> supportedCultures, IGeoLocationService svc) :
            base(supportedCultures, svc)
        {

        }
    }

    public class LocalizationHttpHandlerProxy : LocalizationHttpHandler
    {
        public RouteValueDictionary GetCompleteRouteProxy(RouteValueDictionary route) => GetCompleteRoute(route);

        public string DefaultAction { get; set; } = "ACTION";

        public string LocaleRouteTokenNameProxy => LocaleRouteTokenName;

        public HttpCookie GetLocaleCookie() => new HttpCookie(LocaleCookieName);

        protected override string GetDefaultAction(string controllerName) => DefaultAction;

        public RouteValueDictionary ProcessRouteProxy(HttpCookie cookie) => ProcessRoute(cookie);

        public LocalizationHttpHandlerProxy(HttpRequestBase request, List<CultureInfo> supportedCultures) :
            base(new RequestContext(), new RequestContextLocaleProxy(supportedCultures, Mocks.GetMockGeoLocationService(new Dictionary<IPAddress, IEnumerable<CultureInfo>>()).Object))
        {
        }
    }

    [TestFixture]
    public class GetCompleteRouteTests
    {

        private readonly LocalizationHttpHandlerProxy _handler = new LocalizationHttpHandlerProxy(Mocks.GetMockRequest().Object, new List<CultureInfo>() { CultureInfo.GetCultureInfo("en-US") });

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
            var req = Mocks.GetMockRequest();
            CultureInfo supportedLocale = CultureInfo.GetCultureInfo("en-US");
            LocalizationHttpHandlerProxy handler = new LocalizationHttpHandlerProxy(req.Object, new List<CultureInfo>() { supportedLocale });
            HttpCookie cookie = handler.GetLocaleCookie();
            handler.ProcessRouteProxy(cookie);

            Assert.AreEqual(supportedLocale.Name, cookie.Values[handler.LocaleRouteTokenNameProxy]);
        }
    }
}
