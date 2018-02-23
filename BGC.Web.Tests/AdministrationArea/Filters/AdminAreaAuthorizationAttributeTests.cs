using BGC.Core;
using BGC.Web.Areas.Administration;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Filters.AdminAreaAuthorizationAttributeTests
{
    [TestFixture]
    public class HasNecessaryPermissionsTests
    {
        AdminAreaAuthorizationAttribute _attr;

        [OneTimeSetUp]
        public void Setup()
        {
            _attr = new AdminAreaAuthorizationAttribute();
        }

        [Test]
        public void ReturnsTrueIfNoPermissionsAreRequired1()
        {
            Assert.IsTrue(_attr.HasNecessaryPermissions(new string[] { "some_permission" }, new PermissionsAttribute()));
        }

        [Test]
        public void ReturnsTrueIfNoPermissionsAreRequired2()
        {
            Assert.IsTrue(_attr.HasNecessaryPermissions(new string[] { "some_permission" }, null));
        }

        [Test]
        public void ReturnsFalseIfNoPermissionsAreAvailable2()
        {
            Assert.IsFalse(_attr.HasNecessaryPermissions(Enumerable.Empty<string>(), new PermissionsAttribute()));
        }

        [Test]
        public void ReturnsTrueIfPermissionsMatch()
        {
            var requiredPermissions = new PermissionsAttribute(nameof(IArticleManagementPermission));
            var availablePermissions = new[] { nameof(IArticleManagementPermission), nameof(ISendInvitePermission) };
            Assert.IsTrue(_attr.HasNecessaryPermissions(availablePermissions, requiredPermissions));
        }
    }

    [TestFixture]
    public class GetPermissionsTests
    {
        public const string METHOD_PERMISSION = "method_permission";
        public const string CLASS_PERMISSION = "class_permission";

        public class TestControllerClass_NoBasePermission
        {
            [Permissions(METHOD_PERMISSION)]
            public void ActionMethod1()
            {

            }
        }

        [Permissions(CLASS_PERMISSION)]
        public class TestControllerClass_BasePermission
        {
            public void ActionMethod1()
            {
            }

            [Permissions(METHOD_PERMISSION)]
            public virtual void ActionMethod2()
            {
            }
        }

        public class TestControllerClass_Inheriting : TestControllerClass_BasePermission
        {
            public class Nested
            {
                public void ActionMethod3()
                {
                }
            }
        }

        [Test]
        public void GetsPermissionsFromMethod_NoBasePermissions()
        {
            MethodInfo permMethod = typeof(TestControllerClass_NoBasePermission).GetMethod(nameof(TestControllerClass_NoBasePermission.ActionMethod1));
            IEnumerable<string> reqPermissions = AdminAreaAuthorizationAttribute.GetRequiredPermissions(permMethod).PermissionTypes;

            Assert.AreEqual(1, reqPermissions.Count());
            Assert.AreEqual(METHOD_PERMISSION, reqPermissions.First());
        }

        [Test]
        public void GetsPermissionsFromMethod_WithBasePermissions1()
        {
            MethodInfo noPermMethod = typeof(TestControllerClass_BasePermission).GetMethod(nameof(TestControllerClass_BasePermission.ActionMethod1));
            IEnumerable<string> reqPermissions = AdminAreaAuthorizationAttribute.GetRequiredPermissions(noPermMethod).PermissionTypes;

            Assert.AreEqual(1, reqPermissions.Count());
            Assert.AreEqual(CLASS_PERMISSION, reqPermissions.First());
        }

        [Test]
        public void GetsPermissionsFromMethod_WithBasePermissions2()
        {
            MethodInfo permMethod = typeof(TestControllerClass_BasePermission).GetMethod(nameof(TestControllerClass_BasePermission.ActionMethod2));
            IEnumerable<string> reqPermissions = AdminAreaAuthorizationAttribute.GetRequiredPermissions(permMethod).PermissionTypes;

            Assert.AreEqual(1, reqPermissions.Count());
            Assert.AreEqual(METHOD_PERMISSION, reqPermissions.First());
        }

        [Test]
        public void GetsPermissionsFromMethod_WithInheritance1()
        {
            MethodInfo permMethod = typeof(TestControllerClass_Inheriting).GetMethod(nameof(TestControllerClass_Inheriting.ActionMethod2));
            IEnumerable<string> reqPermissions = AdminAreaAuthorizationAttribute.GetRequiredPermissions(permMethod).PermissionTypes;

            Assert.AreEqual(1, reqPermissions.Count());
            Assert.AreEqual(METHOD_PERMISSION, reqPermissions.First());
        }

        [Test]
        public void GetsPermissionsFromMethod_WithNestedType()
        {
            IEnumerable<string> reqPermissions = null;

            var timeout = new CancellationTokenSource();
            timeout.CancelAfter(TimeSpan.FromSeconds(1));
            try
            {
                Task testRun = Task.Run(() =>
                {
                    MethodInfo permMethod = typeof(TestControllerClass_Inheriting.Nested).GetMethod(nameof(TestControllerClass_Inheriting.Nested.ActionMethod3));
                    reqPermissions = AdminAreaAuthorizationAttribute.GetRequiredPermissions(permMethod).PermissionTypes;
                });
                testRun.Wait(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                Assert.Fail("The test took too long to execute. This could mean an infinite loop has been encountered.");
            }
                        
            Assert.AreEqual(1, reqPermissions.Count());
            Assert.AreEqual(CLASS_PERMISSION, reqPermissions.First());
        }
    }

    [TestFixture]
    public class HandleUnauthorizedRequestTests
    {
        [Test]
        public void PreservesOriginalURLAfterAuthentication_NoParameters()
        {
            Uri originalRequestUrl = new Uri("http://localhost/action");

            var attribute = new AdminAreaAuthorizationAttribute();
            var filterContext = new AuthorizationContext();
            var mockRequest = GetMockRequestBase(MockBehavior.Loose);
            mockRequest.Setup(x => x.CurrentExecutionFilePath).Returns(originalRequestUrl.AbsolutePath);
            mockRequest.Setup(x => x.QueryString).Returns(new NameValueCollection());
            filterContext.HttpContext = GetMockHttpContextBase(mockRequest.Object).Object;
            filterContext.HttpContext.User = GetMockUser("test", true).Object;
            filterContext.Controller = GetMockController().Object;

            attribute.HandleUnauthorizedRequestInternal(filterContext, "http://localhost/someLoginUrl");

            string actualReturnUrl = filterContext.Controller.TempData[WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl].ToString();
            Assert.AreEqual(originalRequestUrl.PathAndQuery, actualReturnUrl);
        }

        [Test]
        public void PreservesOriginalURLAfterAuthentication_OneParameter()
        {
            Uri originalRequestUrl = new Uri("http://localhost/action?param1=1");
            var query = new NameValueCollection();
            query.Add("param1", "1");

            var attribute = new AdminAreaAuthorizationAttribute();
            var filterContext = new AuthorizationContext();
            var mockRequest = GetMockRequestBase(MockBehavior.Loose);
            mockRequest.Setup(x => x.CurrentExecutionFilePath).Returns(originalRequestUrl.AbsolutePath);
            mockRequest.Setup(x => x.QueryString).Returns(query);
            filterContext.HttpContext = GetMockHttpContextBase(mockRequest.Object).Object;
            filterContext.HttpContext.User = GetMockUser("test", true).Object;
            filterContext.Controller = GetMockController().Object;

            attribute.HandleUnauthorizedRequestInternal(filterContext, "http://localhost/someLoginUrl");

            Assert.AreEqual(originalRequestUrl.PathAndQuery, (filterContext.Controller.TempData[WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl]));
        }

        [Test]
        public void PreservesOriginalURLAfterAuthentication_MultipleParameters()
        {
            Uri originalRequestUrl = new Uri("http://localhost/action?param1=1&param2=self");
            var query = new NameValueCollection();
            query.Add("param1", "1");
            query.Add("param2", "self");

            var attribute = new AdminAreaAuthorizationAttribute();
            var filterContext = new AuthorizationContext();
            var mockRequest = GetMockRequestBase(MockBehavior.Loose);
            mockRequest.Setup(x => x.CurrentExecutionFilePath).Returns(originalRequestUrl.AbsolutePath);
            mockRequest.Setup(x => x.QueryString).Returns(query);
            filterContext.HttpContext = GetMockHttpContextBase(mockRequest.Object).Object;
            filterContext.HttpContext.User = GetMockUser("test", true).Object;
            filterContext.Controller = GetMockController().Object;

            attribute.HandleUnauthorizedRequestInternal(filterContext, "http://localhost/someLoginUrl");

            Assert.AreEqual(originalRequestUrl.PathAndQuery, (filterContext.Controller.TempData[WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl]));
        }
    }
}
