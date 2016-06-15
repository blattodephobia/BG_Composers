using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public.Controllers
{
	public partial class MainController : Controller
    {
		private IComposerDataService ComposersService { get; set; }

		public MainController(IComposerDataService composersService)
		{
			this.ComposersService = composersService;
		}

		public virtual ActionResult Index()
        {
            return View();
        }
    }
}
