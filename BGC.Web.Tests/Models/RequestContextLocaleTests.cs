using BGC.Core;
using BGC.Web.Models;
using Moq;
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
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.Models
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ChecksAppProfileForValidData()
        {
            Assert.Throws<InvalidOperationException>(() => new RequestContextLocale(new WebApplicationSettings(), new HttpCookie("key")));
        }
    }

    [TestFixture]
    public class FromRequestTests
    {
        [Test]
        public void ReturnsValidDependencyValueFromUnknownIp()
        {
            Mock<HttpRequestBase> mockRequest = GetMockRequestBase(MockBehavior.Loose);
            mockRequest.Setup(x => x.UserHostAddress).Returns("127.0.0.1");
            mockRequest.Setup(x => x.RequestContext).Returns(new RequestContext()
            {
                RouteData = new RouteData()
            });

            RequestContextLocale reqLocale = RequestContextLocale.FromRequest(
                appProfile: GetStandardAppProfile(),
                geoLocationService: GetMockGeoLocationService(new Dictionary<IPAddress, IEnumerable<CultureInfo>>()).Object,
                request: mockRequest.Object,
                cookieStore: new HttpCookie("someCookie"));

            Assert.AreEqual(new CultureInfo("en-US"), reqLocale.EffectiveValue);
        }
    }
}
