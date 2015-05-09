using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Public.Controllers
{
	public partial class MainController : ComposersControllerBase
    {
		public MainController(IUnitOfWork unitOfWork) :
			base(unitOfWork)
		{

		}

		public virtual ActionResult Index()
        {
            return View();
        }
    }
}
