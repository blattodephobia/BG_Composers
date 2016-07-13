using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public.Controllers
{
    public partial class ResourcesController : Controller
    {
        private IMediaStorageService storageService;

        public ResourcesController(IMediaStorageService storageService)
        {
            Shield.ArgumentNotNull(storageService, nameof(storageService));

            this.storageService = storageService;
        }

        [HttpPost]
        public virtual ActionResult Upload(HttpPostedFileBase file)
        {
            Guid id = storageService.AddMedia(new ContentType(file.ContentType), file.InputStream);
            return Content(id.ToString());
        }
    }
}
