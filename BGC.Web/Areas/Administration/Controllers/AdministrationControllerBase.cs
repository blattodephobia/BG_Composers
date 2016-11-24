using BGC.Core;
using BGC.Web.Controllers;
using CodeShield;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [AdminAreaAuthorization]
    public class AdministrationControllerBase : BgcControllerBase
    {
        private BgcUserManager userManager;
        [Dependency]
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
                    this.user = UserManager.FindByName(base.User.Identity.Name);
                    userInitialized = true;
                }

                return this.user;
            }
        }
    }
}