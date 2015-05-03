using BGC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
	public partial class AccountController : AdministrationControllerBase
    {
		public virtual ActionResult Users()
        {
			LocalizationDictionary dict = new LocalizationDictionary();
			string s = dict.AdministrationArea.Administration.Users.Ok;
            return View();
        }

		[AllowAnonymous]
		public virtual ActionResult Login(string returnUrl = "")
        {
			return this.View();
        }

		[AllowAnonymous]
        [HttpPost]
		public virtual ActionResult Login(LoginViewModel model)
        {
            return new EmptyResult();
        }
    }
}
