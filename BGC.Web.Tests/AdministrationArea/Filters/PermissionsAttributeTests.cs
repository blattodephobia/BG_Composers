using BGC.Core;
using BGC.Web.Areas.Administration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Web.Tests.AdministrationArea.Filters
{
    [TestFixture]
    public class IsAuthorizedTests
    {
        [Test]
        public void AuthorizesFullSetOfPermissions()
        {
            var attr = new PermissionsAttribute(typeof(UserSettingsPermission), typeof(SendInvitePermission));
            Assert.IsTrue(attr.IsAuthorized(new IPermission[] { new UserSettingsPermission(), new SendInvitePermission() }));
        }

        [Test]
        public void AuthorizesWithExtraPermissions()
        {
            var attr = new PermissionsAttribute(typeof(UserSettingsPermission), typeof(SendInvitePermission));
            Assert.IsTrue(attr.IsAuthorized(new IPermission[] { new UserSettingsPermission(), new SendInvitePermission(), new ArticleManagementPermission() }));
        }

        [Test]
        public void AuthorizesAnonymous()
        {
            var attr = new PermissionsAttribute();
            Assert.IsTrue(attr.IsAuthorized(new IPermission[] { new UserSettingsPermission(), new SendInvitePermission(), new ArticleManagementPermission() }));
        }

        [Test]
        public void NoAuthorizationForPartialSetOfPermissions()
        {
            var attr = new PermissionsAttribute(typeof(UserSettingsPermission), typeof(SendInvitePermission));
            Assert.IsFalse(attr.IsAuthorized(new IPermission[] { new SendInvitePermission() }));
        }

        [Test]
        public void ThrowsExceptionIfNoPermissions_1()
        {
            var attr = new PermissionsAttribute(typeof(UserSettingsPermission), typeof(SendInvitePermission));
            Assert.Throws<InvalidOperationException>(() => attr.IsAuthorized(new IPermission[0]));
        }

        [Test]
        public void ThrowsExceptionIfNoPermissions_2()
        {
            var attr = new PermissionsAttribute(typeof(UserSettingsPermission), typeof(SendInvitePermission));
            Assert.Throws<InvalidOperationException>(() => attr.IsAuthorized(null));
        }
    }
}
