using BGC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
    public class AdministrationController : AdministrationControllerBase
    {
        public ActionResult Users()
        {
            return View();
        }

    }
}
