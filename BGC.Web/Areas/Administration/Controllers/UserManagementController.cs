using BGC.Core;
using BGC.Core.Exceptions;
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
            if (User.FindPermission<SendInvitePermission>() == null)
            {
                return new HttpUnauthorizedResult();
            }
            else
            {
                vm = vm ?? new SendInvitePermissionViewModel();
                vm.AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                return View(vm);
            }
        }

        [HttpPost]
        [ActionName(nameof(SendInvite))]
        public virtual ActionResult SendInvite_Post(SendInvitePermissionViewModel invitation)
        {
            try
            {
                Invitation invitationResult = UserManager.Invite(User, invitation.Email, invitation.AvailableRoles.Select(s => new BgcRole(s)));
                UserManager.EmailService.Send(new IdentityMessage()
                {
                    Destination = invitationResult.Email,
                    Subject = "BG Composers invitation",
                    Body = $"{Url.ActionAbsolute(MVC.AdministrationArea.UserManagement.Register())}?{Expressions.GetQueryString(() => Register(invitationResult.Id))}"
                });
                return SendInvite(new SendInvitePermissionViewModel() { IsPreviousInvitationSent = true });
            }
            catch (UnauthorizedAccessException)
            {
                return new HttpUnauthorizedResult();
            }
            catch (DuplicateEntityException)
            {
                return SendInvite(new SendInvitePermissionViewModel() { ErrorMessages = new[] { LocalizationKeys.Administration.UserManagement.SendInvite.UserExists } });
            }
        }

        [AllowAnonymous]
        public virtual ActionResult Register(Guid invitation)
        {
            if (TempData.ContainsKey(nameof(RegisterViewModel)))
            {
                return View(TempData[nameof(RegisterViewModel)]);
            }
            
            BgcUser user = UserManager.Create(invitation);
            if (user != null)
            {
                return View(new RegisterViewModel() { InvitationId = invitation, Roles = user.Roles.Select(ur => ur.Role.Name).ToList() });
            }
            else
            {
                return HttpNotFound();
            }
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
