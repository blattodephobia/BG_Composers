using BGC.Core;
using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
	[AdminAreaAuthorization(nameof(AdministratorRole))]
    public class AdministrationController : AccountController
    {
        public virtual ActionResult Users()
        {
            return View();
        }
    }
}
