using BGC.Core;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [AdminAreaAuthorization]
    public class AdministrationControllerBase : Controller
    {
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
                    this.user = UserManager.FindByName(base.User.Identity.Name);
                    userInitialized = true;
                }

                return this.user;
            }
        }
    }
}