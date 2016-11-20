﻿using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static BGC.Core.BgcUserTokenProvider;

namespace BGC.Web.Areas.Administration.Controllers
{
	public partial class AccountController : AuthenticationController
    {
        private DiscoveredTypes typeDiscovery;
        
        protected AccountController()
        {
        }

        public AccountController(SignInManager<BgcUser, long> signInManager) :
            base(signInManager)
        {            
            this.typeDiscovery = TypeDiscovery.Discover(GetType(), a => a == Assembly.GetExecutingAssembly());
        }

        public virtual ActionResult Activities()
        {
            var validActivities = (from viewModel in typeDiscovery.DiscoveredTypesInheritingFrom<PermissionViewModelBase>()
                                   from mapAttribute in viewModel.GetCustomAttributes<MappableWithAttribute>()
                                   join permission in User.GetPermissions() on mapAttribute.RelatedType equals permission.GetType()
                                   where viewModel.GetCustomAttribute<GeneratedCodeAttribute>() != null
                                   select Activator.CreateInstance(viewModel) as PermissionViewModelBase).ToList();

            foreach (PermissionViewModelBase vm in validActivities.Where(activity => activity.ActivityAction != null))
            {
                vm.ActivityUrl = Url.RouteUrl(vm.ActivityAction.GetRouteValueDictionary());
            }
            return View(validActivities);
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> ResetPassword(string email, string token)
        {
            BgcUser user = await UserManager.FindByEmailAsync(email);
            if (user != null && await UserManager.UserTokenProvider.ValidateAsync(TokenPurposes.ResetPassword, token, UserManager, user))
            {
                return View(new PasswordResetViewModel() { Email = email, Token = token });
            }
            else
            {
                return View(new PasswordResetViewModel() { ErrorMessageKey = LocalizationKeys.Administration.Account.PasswordReset.UnknownEmailError });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(nameof(ResetPassword))]
        public virtual async Task<ActionResult> ResetPassword_Post(PasswordResetViewModel vm)
        {
            BgcUser user = await UserManager.FindByEmailAsync(vm.Email);
            if (user != null)
            {
                IdentityResult resetPasswordResult = await UserManager.ResetPasswordAsync(user.Id, vm.Token, vm.NewPassword);
                if (resetPasswordResult.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false, false);
                    return RedirectToAction(nameof(Activities));
                }
                else
                {
                    return View(new PasswordResetViewModel() { ErrorMessageKey = LocalizationKeys.Administration.Account.PasswordReset.UnknownError });
                }
            }
            else
            {
                return View(new PasswordResetViewModel() { ErrorMessageKey = LocalizationKeys.Administration.Account.PasswordReset.UnknownEmailError });
            }
        }

        [AllowAnonymous]
        public virtual ActionResult RequestPasswordReset(RequestPasswordResetViewModel vm = null)
        {
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(nameof(RequestPasswordReset))]
        public virtual async Task<ActionResult> RequestPasswordReset_Post(PasswordResetViewModel vm)
        {
            BgcUser user = UserManager.FindByEmail(vm.Email);
            if (user != null)
            {
                string token = UserManager.GeneratePasswordResetToken(user.Id);
                await UserManager.EmailService.SendAsync(new IdentityMessage()
                {
                    Destination = vm.Email,
                    Subject = "Password reset request",
                    Body = $"{Url.ActionAbsolute(ResetPassword())}?{Expressions.GetQueryString(() => ResetPassword(vm.Email, token))}"
                });
            }
            // if user == null, we don't want to disclose that the email is not found, due to security reasons
            return RequestPasswordReset(new RequestPasswordResetViewModel() { IsEmailSent = true });
        }
    }
}
