using BGC.Core;
using BGC.Web.Areas.Administration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
