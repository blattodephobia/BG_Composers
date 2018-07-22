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
        public static Uri GetLocalResourceUri(UrlHelper helper, Guid id)
        {
            var uriBuilder = new UriBuilder()
            {
                Path = helper.Action(MVC.Public.Resources.ActionNames.Get, MVC.Public.Resources.Name),
                Query = Expressions.GetQueryString((ResourcesController rc) => rc.Get(id)),
            };

            Uri requestUrl = helper.RequestContext.HttpContext.Request.Url;
            if (!requestUrl.IsDefaultPort)
            {
                uriBuilder.Port = requestUrl.Port;
            }

            return uriBuilder.Uri;
        }

        private IMediaService storageService;

        public ResourcesController(IMediaService storageService)
        {
            Shield.ArgumentNotNull(storageService, nameof(storageService));

            this.storageService = storageService;
        }

        public virtual ActionResult Get(Guid resourceId)
        {
            MultimediaContent media = storageService.GetMedia(resourceId);
            return (media == null)
                ? HttpNotFound() as ActionResult
                : File(media.Data, media.Metadata.MimeType.ToString(), media.Metadata.OriginalFileName) as ActionResult;
        }

        [HttpPost]
        public virtual ActionResult Upload(HttpPostedFileBase file)
        {
            Guid id = storageService.AddMedia(new ContentType(file.ContentType), file.InputStream, file.FileName);            
            Uri resourceUri = GetLocalResourceUri(Url, id);
            return Content(resourceUri.AbsoluteUri);
        }
    }
}
