using BGC.Core;
using BGC.Web.Areas.Administration;
using BGC.Web.Areas.Administration.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TestUtils;
using static TestUtils.Mocks;

namespace BGC.Web.Tests.AdministrationArea.Controllers
{
    [TestFixture]
    public class LoginTests
    {
        [Test]
        public void RedirectsToActivitiesIfAuthenticated()
        {
            var user = new BgcUser() { UserName = "admin" };
            var baseUrl = new Uri("http://localhost");
            Mock<HttpRequestBase> mockReq = GetMockRequestBase(MockBehavior.Strict);
            mockReq.SetupGet(x => x.ApplicationPath).Returns("/");
            mockReq.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            mockReq.SetupGet(x => x.ServerVariables).Returns(new NameValueCollection());

            Mock<HttpResponseBase> mockResp = GetMockResponseBase(MockBehavior.Strict);
            mockResp.Setup(x => x.ApplyAppPathModifier(It.IsNotNull<string>())).Returns((string path) =>
            {
                var builder = new UriBuilder(baseUrl);
                builder.Path = path;
                return builder.Uri.ToString();
            });

            Mock<HttpContextBase> mockContext = GetMockHttpContextBase(mockReq.Object, mockResp.Object);
            mockContext.SetupGet(c => c.User).Returns(GetMockUser(user.UserName).Object);

            Mock<RequestContext> mockReqContext = GetMockRequestContext(mockReq.Object, mockResp.Object);
            var reg = new AdministrationAreaRegistration();
            var regContext = new AreaRegistrationContext(reg.AreaName, new RouteCollection());
            reg.RegisterArea(regContext);

            mockReqContext.SetupGet(x => x.RouteData).Returns(new RouteData(regContext.Routes.Single(), null));
            mockReqContext.SetupGet(x => x.HttpContext).Returns(mockContext.Object);

            var ctrl = new AuthenticationController();
            ctrl.UserManager = GetMockUserManager(user, GetMockUserStore(user).Object, GetMockTokenProvider("token", user).Object).Object;
            ctrl.ControllerContext = new ControllerContext(mockContext.Object, new RouteData(), ctrl);
            ctrl.Url = new UrlHelper(mockReqContext.Object, regContext.Routes);

            var authUserRedirect = ctrl.Login() as RedirectResult;
            Assert.IsNotNull(authUserRedirect);
            Assert.AreEqual(
                expected: new Uri(baseUrl, $"areas/{reg.AreaName}/{MVC.Administration.Account.Name}/{MVC.Administration.Account.ActionNames.Activities}").ToString(),
                actual: authUserRedirect.Url);
        }
    }
}
