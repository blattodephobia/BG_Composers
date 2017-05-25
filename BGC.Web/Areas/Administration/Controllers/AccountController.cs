using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
	public partial class AccountController : AdministrationControllerBase
    {
        private DiscoveredTypes _typeDiscovery;
        private readonly IComposerDataService _composerService;
        
        public AccountController(IComposerDataService composerService)
        {
            Shield.ArgumentNotNull(composerService).ThrowOnError();

            _typeDiscovery = TypeDiscovery.Discover(GetType(), a => a == Assembly.GetExecutingAssembly());
            _composerService = composerService;
        }

        public virtual ActionResult Activities()
        {
            var validActivities = (from viewModel in _typeDiscovery.DiscoveredTypesInheritingFrom<PermissionViewModelBase>()
                                   from mapAttribute in viewModel.GetCustomAttributes<MappableWithAttribute>()
                                   join permission in User.GetPermissions() on mapAttribute.RelatedType equals permission.GetType()
                                   where viewModel.GetCustomAttribute<GeneratedCodeAttribute>() != null
                                   select Activator.CreateInstance(viewModel) as PermissionViewModelBase).ToList();

            foreach (PermissionViewModelBase vm in validActivities.Where(activity => activity.ActivityAction != null))
            {
                vm.ActivityUrl = Url.RouteUrl(vm.ActivityAction.GetRouteValueDictionary());
            }

            return View(new ActivitiesViewModel()
            {
                PermittedActions = validActivities,
                PublishedComposersCount = _composerService.GetAllComposers().Count
            });
        }

        public virtual ActionResult ChangePassword(ChangePasswordViewModel vm = null)
        {
            return View(vm ?? new ChangePasswordViewModel());
        }

        [HttpPost]
        [ActionName(nameof(ChangePassword))]
        public virtual async Task<ActionResult> ChangePassword_Post(ChangePasswordViewModel vm)
        {
            if (!ModelState.IsValidField(nameof(ChangePasswordViewModel.ConfirmPassword)))
            {
                return ChangePassword(new ChangePasswordViewModel()
                {
                    ErrorMessages = new[] { Localize(LocalizationKeys.Administration.Account.ChangePassword.PasswordsMismatch) }
                });
            }

            if (!UserManager.CheckPassword(User, vm.CurrentPassword))
            {
                return ChangePassword(new ChangePasswordViewModel()
                {
                    ErrorMessages = new[] { Localize(LocalizationKeys.Administration.Account.ChangePassword.WrongPassword) }
                });
            }

            IdentityResult opResult = await UserManager.ChangePasswordAsync(User.Id, vm.CurrentPassword, vm.NewPassword);
            if (opResult.Succeeded)
            {
                return RedirectToAction(nameof(Activities));
            }
            else
            {
                return ChangePassword(new ChangePasswordViewModel()
                {
                    ErrorMessages = new[] { Localize(LocalizationKeys.Administration.Account.ChangePassword.UnknownError) }
                });
            }
        }
        
        public virtual ActionResult SetLocale(string locale)
        {
            CultureInfo newLocale = null;
            try
            {
                newLocale = CultureInfo.GetCultureInfo(locale);

                UserProfile.PreferredLocale = newLocale;
                UserLocale.DbSetting.SetValue(newLocale);
                UserLocale.CookieSetting.SetValue(newLocale);

                UserManager.Update(User);
            }
            catch (CultureNotFoundException)
            {
            }

            return Redirect(Request.UrlReferrer.AbsolutePath);
        }

        [AllowAnonymous]
        public virtual async Task<ActionResult> ResetPassword(string referrer, string token)
        {
            if (TempData.ContainsKey(nameof(PasswordResetViewModel)))
            {
                return View(TempData[nameof(PasswordResetViewModel)]);
            }

            try
            {
                string email = Decrypt(referrer);
                BgcUser user = await UserManager.FindByEmailAsync(email);
                if (user != null && await UserManager.UserTokenProvider.ValidateAsync(TokenPurposes.ResetPassword, token, UserManager, user))
                {
                    return View(new PasswordResetViewModel()
                    {
                        Email = email,
                        Token = token
                    });
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine($"An unsuccessful password reset attempt occurred: {e.Message}");
            }

            return View(new PasswordResetViewModel()
            {
                RenderErrorsOnly = true,
                ErrorMessages = new[] { LocalizationKeys.Administration.Account.ResetPassword.UnknownEmailError }
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(nameof(ResetPassword))]
        public virtual async Task<ActionResult> ResetPassword_Post(PasswordResetViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData.Add(nameof(PasswordResetViewModel), vm.WithModelStateErrors(ModelState));
                return RedirectToAction(nameof(ResetPassword), new { referrer = Encrypt(vm.Email), token = vm.Token });
            }

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
                    return View(new PasswordResetViewModel()
                    {
                        ErrorMessages = new[] { LocalizationKeys.Administration.Account.ResetPassword.UnknownError }
                    });
                }
            }
            else
            {
                return View(new PasswordResetViewModel()
                {
                    ErrorMessages = new[] { LocalizationKeys.Administration.Account.ResetPassword.UnknownEmailError }
                });
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
                string emailEncrypted = Encrypt(vm.Email);
                await UserManager.EmailService.SendAsync(new IdentityMessage()
                {
                    Destination = vm.Email,
                    Subject = "Password reset request",
                    Body = $"{Url.ActionAbsolute(ResetPassword())}?{Expressions.GetQueryString(() => ResetPassword(emailEncrypted, token))}"
                });
            }
            // if user == null, we don't want to disclose that the email is not found, due to security reasons
            return RequestPasswordReset(new RequestPasswordResetViewModel() { IsEmailSent = true });
        }
    }
}
