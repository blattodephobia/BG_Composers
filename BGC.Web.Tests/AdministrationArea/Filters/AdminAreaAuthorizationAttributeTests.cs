using BGC.Core;
using BGC.Web.Areas.Administration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            public void ActionMethod2()
            {
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
    }
}
