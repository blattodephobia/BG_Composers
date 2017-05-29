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
    public partial class UserManagementController : AdministrationControllerBase
    {
        private BgcRoleManager _roleManager;

        [HttpGet]
        [Permissions(nameof(ISendInvitePermission))]
        public virtual ActionResult SendInvite(SendInvitePermissionViewModel vm = null)
        {
            if (User.FindPermission<SendInvitePermission>() == null)
            {
                return new HttpUnauthorizedResult();
            }
            else
            {
                vm = vm ?? new SendInvitePermissionViewModel();
                vm.AvailableRoles = vm.AvailableRoles ?? _roleManager.Roles.Select(r => r.Name).ToList();
                return View(vm);
            }
        }

        [HttpPost]
        [ActionName(nameof(SendInvite))]
        [Permissions(nameof(ISendInvitePermission))]
        public virtual ActionResult SendInvite_Post(SendInvitePermissionViewModel invitation)
        {
            try
            {
                Invitation invitationResult = UserManager.Invite(User, invitation.Email, invitation.AvailableRoles.Select(s => new BgcRole(s)));
                UserManager.EmailService.Send(new IdentityMessage()
                {
                    Destination = invitationResult.Email,
                    Subject = "BG Composers invitation",
                    Body = $"{Url.ActionAbsolute(MVC.Administration.UserManagement.Register())}?{Expressions.GetQueryString(() => Register(invitationResult.Id))}"
                });
                invitation.IsPreviousInvitationSent = true;
                return RedirectToAction(nameof(SendInvite));
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
            
            Invitation dbInvitation = UserManager.FindInvitation(invitation);
            if (dbInvitation != null)
            {
                return View(new RegisterViewModel()
                {
                    InvitationId = invitation,
                    Roles = dbInvitation.AvailableRoles.Select(s => s.Name).ToList()
                });
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
                TempData.Add(nameof(RegisterViewModel), vm.WithModelStateErrors(ModelState));
                return RedirectToAction(nameof(Register), new { invitation = vm.InvitationId });
            }

            try
            {
                var user = UserManager.Create(vm.InvitationId, vm.UserName, vm.NewPassword);
                SignInManager.SignIn(user, false, false);
                return RedirectToAction(MVC.Administration.Account.Activities());
            }
            catch (EntityNotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (DuplicateEntityException)
            {
                vm.ErrorMessages = new[] { LocalizationKeys.Administration.UserManagement.Register.UserNameInUse };
                TempData.Add(nameof(RegisterViewModel), vm.WithModelStateErrors(ModelState));

                return RedirectToAction(nameof(Register), new { invitation = vm.InvitationId });
            }
        }

        public UserManagementController(BgcRoleManager roleManager)
        {
            _roleManager = Shield.ArgumentNotNull(roleManager, nameof(roleManager)).GetValueOrThrow();
        }
    }
}
