using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
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
        private IUserManagementService managementService;
        private BgcRoleManager roleManager;

        [HttpGet]
        public virtual ActionResult SendInvite(SendInvitePermissionViewModel vm = null)
        {
            vm = vm ?? new SendInvitePermissionViewModel();
            vm.AvailableRoles = roleManager.Roles.Select(r => r.Name).ToList();
            return View(vm);
        }

        [HttpPost]
        [ActionName(nameof(SendInvite))]
        public virtual ActionResult SendInvite_Post(SendInvitePermissionViewModel invitation)
        {
            Invitation invitationResult = this.managementService.Invite(invitation.Email, invitation.AvailableRoles.Select(s => new BgcRole(s)));
            return SendInvite();
            //using (var emailClient = new SmtpClient())
            //{
            //    emailClient.Credentials = new NetworkCredential()
            //    {
            //        UserName = "",
            //        Password = ""
            //    };
            //    emailClient.Host = "";
            //    emailClient.Port = 587;
            //    emailClient.EnableSsl = true;
            //    emailClient.SendMailAsync(new MailMessage("", invitation.Email, "BGC editor invitation", "body")).RunSynchronously();
            //    return SendInvite(new SendInvitePermissionViewModel() { IsPreviousInvitationSent = true });
            //}
        }

        public UserManagementController(IUserManagementService managementService, BgcRoleManager roleManager)
        {
            this.managementService = Shield.ArgumentNotNull(managementService, nameof(managementService)).GetValueOrThrow();
            this.roleManager = Shield.ArgumentNotNull(roleManager, nameof(roleManager)).GetValueOrThrow();
        }
    }
}
