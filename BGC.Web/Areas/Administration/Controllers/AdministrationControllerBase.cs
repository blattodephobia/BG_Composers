using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
	[Authorize(Roles="Administrator")]
    public abstract class AdministrationControllerBase : Controller
    {
    }
}
