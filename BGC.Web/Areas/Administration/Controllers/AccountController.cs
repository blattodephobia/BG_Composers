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
			return this.View();
        }
    }
}
