using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace BGC.Web.Areas.Public.Controllers
{
    public partial class ResourcesController : Controller
    {
        private IMediaService storageService;

        public ResourcesController(IMediaService storageService)
        {
            Shield.ArgumentNotNull(storageService, nameof(storageService));

            this.storageService = storageService;
        }

        public virtual ActionResult Get(Guid resourceId)
        {
            MediaTypeInfo media = storageService.GetMedia(resourceId);
            return (media == null)
                ? HttpNotFound() as ActionResult
                : File(media.Content, media.MimeType.ToString(), media.OriginalFileName) as ActionResult;
        }

        [HttpPost]
        public virtual ActionResult Upload(HttpPostedFileBase file)
        {
            Guid id = storageService.AddMedia(new ContentType(file.ContentType), file.InputStream, file.FileName);
            var urlBuilder = new UriBuilder(Request.Url.AbsoluteUri)
            {
                Path = Url.Action(MVC.Public.Resources.ActionNames.Get, MVC.Public.Resources.Name),
                Query = Expressions.GetQueryString(() => Get(id)),
            };
            return Content(urlBuilder.Uri.AbsoluteUri);
        }
    }
}
