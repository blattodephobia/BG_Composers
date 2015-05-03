using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
	public partial class AuthenticationController : AdministrationControllerBase
    {
		[AllowAnonymous]
		public virtual ActionResult Login(string returnUrl = "")
        {
            return View();
        }

    }
}
