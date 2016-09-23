using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [Authorize]
	public partial class AccountController : Controller
    {
        private TypeDiscoveryProvider typeDiscovery;

        private BgcUserManager userManager;
        public BgcUserManager UserManager
        {
            get
            {
                return this.userManager;
            }

            set
            {
                Shield.Assert(value, this.userManager == null, x => new InvalidOperationException("User Manager cannot be set more than once"));
                this.userManager = value;
            }
        }

        private BgcUser user;
        private bool userInitialized;
        public new BgcUser User
        {
            get
            {
                if (!userInitialized && (base.User?.Identity.IsAuthenticated ?? false))
                {
                    user = UserManager.FindByName(base.User.Identity.Name);
                    userInitialized = true;
                }

                return user;
            }
        }

        public AccountController()
        {
            this.typeDiscovery = new TypeDiscoveryProvider(this.GetType(), a => a == Assembly.GetExecutingAssembly());
        }

        public virtual ActionResult Activities()
        {
            var validActivities = from viewModel in typeDiscovery.GetDiscoveredInheritanceChain<PermissionViewModelBase>()
                                  join permission in User.GetPermissions() on viewModel.GetCustomAttribute<>().PermissionType == permission.GetType()
                                  where viewModel.GetCustomAttribute<GeneratedCodeAttribute>() != null
                                  select Activator.CreateInstance(viewModel);
            foreach (PermissionViewModelBase permissionViewModel in validActivities)
            {

            }
        }
    }
}
