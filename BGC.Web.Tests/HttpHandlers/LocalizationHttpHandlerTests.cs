using BGC.Web.HttpHandlers;
using BGC.Web.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using TestUtils;

namespace BGC.Web.Tests.HttpHandlers
{
    [TestFixture]
    public class GetCompleteRouteTests
    {
        private class RequestContextLocaleProxy : LocalizationHttpHandler.RequestContextLocale
        {
            public RequestContextLocaleProxy(List<CultureInfo> supportedCultures, IGeoLocationService svc) :
                base(supportedCultures, svc)
            {

            }
        }
        private class LocalizationHttpHandlerProxy : LocalizationHttpHandler
        {
            public RouteValueDictionary GetCompleteRouteProxy(RouteValueDictionary route) => GetCompleteRoute(route);

            public string DefaultAction { get; set; } = "ACTION";

            protected override string GetDefaultAction(string controllerName) => DefaultAction;

            public LocalizationHttpHandlerProxy(HttpRequestBase request, List<CultureInfo> supportedCultures) :
                base(new RequestContext(), new RequestContextLocaleProxy(supportedCultures, Mocks.GetMockGeoLocationService(new Dictionary<IPAddress, IEnumerable<CultureInfo>>()).Object))
            {
            }
        }

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
}
