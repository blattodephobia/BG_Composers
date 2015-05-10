using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Public.Controllers
{
	public partial class MainController : Controller
    {
		private IComposerEntriesService ComposersService { get; set; }

		public MainController(IComposerEntriesService composersService)
		{
			this.ComposersService = composersService;
		}

		public virtual ActionResult Index()
        {
            return View();
        }
    }
}
