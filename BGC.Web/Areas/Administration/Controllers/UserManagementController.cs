using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [AdminAreaAuthorization(nameof(AdministratorRole))]
    public partial class UserManagementController : AccountController
    {
        private IUserManagementService managementService;
        private BgcRoleManager roleManager;

        [HttpGet]
        [Permissions(typeof(SendInvitePermission))]
        public virtual ActionResult SendInvite()
        {
            return View(new SendInvitePermissionViewModel() { AvailableRoles = roleManager.Roles.Select(r => r.Name).ToList() });
        }

        [HttpPost]
        [Permissions(typeof(SendInvitePermission))]
        [ActionName(nameof(SendInvite))]
        public virtual ActionResult SendInvite_Post(SendInvitePermissionViewModel invitation)
        {
            this.managementService.Invite(invitation.Email);
            return SendInvite();
        }

        public UserManagementController(IUserManagementService managementService, BgcRoleManager roleManager)
        {
            this.managementService = Shield.ArgumentNotNull(managementService, nameof(managementService)).GetValueOrThrow();
            this.roleManager = Shield.ArgumentNotNull(roleManager, nameof(roleManager)).GetValueOrThrow();
        }
    }
}
