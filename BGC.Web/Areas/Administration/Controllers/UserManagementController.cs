using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [AdminAreaAuthorization(nameof(AdministratorRole))]
    public partial class UserManagementController : AccountController
    {
        private BgcRoleManager _roleManager;

        [HttpGet]
        public virtual ActionResult SendInvite(SendInvitePermissionViewModel vm = null)
        {
            vm = vm ?? new SendInvitePermissionViewModel();
            vm.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(vm);
        }

        [HttpPost]
        [ActionName(nameof(SendInvite))]
        public virtual ActionResult SendInvite_Post(SendInvitePermissionViewModel invitation)
        {
            Invitation invitationResult = UserManager.Invite(User, invitation.Email, invitation.AvailableRoles.Select(s => new BgcRole(s)));
            return SendInvite();
        }

        [AllowAnonymous]
        public virtual ActionResult Register(Guid invitation)
        {
            return View(new RegisterViewModel() { InvitationId = invitation });
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(nameof(Register))]
        public virtual ActionResult Register_Post(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData.Add(nameof(RegisterViewModel), vm);
                return RedirectToAction(nameof(Register), new { invitation = vm.InvitationId });
            }

            var user = new BgcUser(vm.UserName);
            UserManager.Create(user, vm.NewPassword);
            SignInManager.SignIn(user, false, false);
            return RedirectToAction(MVC.AdministrationArea.Account.Activities());
        }

        public UserManagementController(BgcRoleManager roleManager, SignInManager<BgcUser, long> signInManager) :
            base(signInManager)
        {
            _roleManager = Shield.ArgumentNotNull(roleManager, nameof(roleManager)).GetValueOrThrow();
        }
    }
}
