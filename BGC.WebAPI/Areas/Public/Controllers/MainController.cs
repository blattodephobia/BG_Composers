using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Public.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Public/Main/

        public ActionResult Index()
        {
            return View();
        }

    }
}
