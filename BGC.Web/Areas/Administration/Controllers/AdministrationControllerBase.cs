using BGC.Core;
using BGC.Utilities;
using CodeShield;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
	[AdminAreaAuthorization(Roles = "Administrator")]
    public abstract class AdministrationControllerBase : Controller
    {
		private UserManager<AspNetUser, long> userManager;
		public UserManager<AspNetUser, long> UserManager
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
    }
}
