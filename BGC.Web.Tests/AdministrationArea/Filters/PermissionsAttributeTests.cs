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
            var permissions = new IPermission[] { new UserSettingsPermission(), new SendInvitePermission() };
            var attr = new PermissionsAttribute(permissions[0].Name, permissions[1].Name);
            Assert.IsTrue(attr.IsAuthorized(permissions.Select(p => p.Name)));
        }

        [Test]
        public void AuthorizesWithExtraPermissions()
        {
            var permissions = new IPermission[] { new UserSettingsPermission(), new SendInvitePermission() };
            var attr = new PermissionsAttribute(permissions[0].Name, permissions[1].Name);
            Assert.IsTrue(attr.IsAuthorized(permissions.Select(p => p.Name)));
        }

        [Test]
        public void AuthorizesAnonymous()
        {
            var permissions = new IPermission[] { new UserSettingsPermission(), new SendInvitePermission(), new ArticleManagementPermission() };
            var attr = new PermissionsAttribute();
            Assert.IsTrue(attr.IsAuthorized(permissions.Select(p => p.Name)));
        }

        [Test]
        public void ThrowsExceptionIfNoPermissions_1()
        {
            var attr = new PermissionsAttribute(nameof(IUserSettingsPermission), nameof(ISendInvitePermission));
            Assert.Throws<InvalidOperationException>(() => attr.IsAuthorized(new IPermission[0]));
        }

        [Test]
        public void ThrowsExceptionIfNoPermissions_2()
        {
            var attr = new PermissionsAttribute(nameof(IUserSettingsPermission), nameof(ISendInvitePermission));
            IEnumerable<IPermission> nullCollection = null;
            Assert.Throws<InvalidOperationException>(() => attr.IsAuthorized(nullCollection));
        }

        [Test]
        public void ThrowsExceptionIfCollectionHasNullElements()
        {
            var attr = new PermissionsAttribute(nameof(IUserSettingsPermission), nameof(ISendInvitePermission));
            IEnumerable<IPermission> nullCollection = new IPermission[] { new UserSettingsPermission(), null };
            Assert.Throws<InvalidOperationException>(() => attr.IsAuthorized(nullCollection));
        }
    }
}
