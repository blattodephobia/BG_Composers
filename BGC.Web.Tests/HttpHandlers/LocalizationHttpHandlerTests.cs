using BGC.Web.HttpHandlers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace BGC.Web.Tests.HttpHandlers
{
    [TestFixture]
    public class GetCompleteRouteTests
    {
        private class LocalizationHttpHanlderProxy : LocalizationHttpHandler
        {
            public RouteValueDictionary GetCompleteRouteProxy(RouteValueDictionary route) => GetCompleteRoute(route);

            public string DefaultAction { get; set; } = "ACTION";

            protected override string GetDefaulAction(string controllerName) => DefaultAction;

            public LocalizationHttpHanlderProxy() :
                base(new RequestContext())
            {
            }
        }

        private readonly LocalizationHttpHanlderProxy _handler = new LocalizationHttpHanlderProxy();

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
