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
	public partial class AccountController : AdministrationControllerBase
    {
        private TypeDiscoveryProvider typeDiscovery;
        
        public AccountController()
        {            
            this.typeDiscovery = new TypeDiscoveryProvider(GetType(), a => a == Assembly.GetExecutingAssembly());
        }

        public virtual ActionResult Activities()
        {
            var validActivities = from viewModel in typeDiscovery.GetDiscoveredInheritanceChain<PermissionViewModelBase>()
                                  from mapAttribute in viewModel.GetCustomAttributes<MappableWithAttribute>()
                                  join permission in User.GetPermissions() on mapAttribute.RelatedType equals permission.GetType()
                                  where viewModel.GetCustomAttribute<GeneratedCodeAttribute>() != null
                                  select Activator.CreateInstance(viewModel) as PermissionViewModelBase;

            return View(validActivities);
        }
    }
}
