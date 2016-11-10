using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
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
	public partial class AccountController : AdministrationControllerBase
    {
        private TypeDiscoveryProvider typeDiscovery;
        
        public AccountController()
        {            
            this.typeDiscovery = new TypeDiscoveryProvider(GetType(), a => a == Assembly.GetExecutingAssembly());
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
        public virtual ActionResult ResetPassword(PasswordResetViewModel vm = null)
        {
            return View(vm);
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName(nameof(ResetPassword))]
        public virtual async Task<ActionResult> ResetPassword_Post(PasswordResetViewModel vm)
        {
            string token = await UserManager.UserTokenProvider.GenerateAsync(TokenPurposes.PasswordReset, UserManager, User);
            User.SetPasswordResetTokenHash(token);
            await UserManager.UpdateAsync(User);
            return RedirectToAction(MVC.AdministrationArea.Authentication.Login(new LoginViewModel() { IsRedirectFromPasswordReset = true }));
        }
    }
}
