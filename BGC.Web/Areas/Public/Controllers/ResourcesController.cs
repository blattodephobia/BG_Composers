﻿using BGC.Core;
using BGC.Core.Services;
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
            if (media == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(media.Content, media.MimeType.ToString(), media.OriginalFileName);
            }
        }

        [HttpPost]
        public virtual ActionResult Upload(HttpPostedFileBase file)
        {
            Guid id = storageService.AddMedia(new ContentType(file.ContentType), file.InputStream, file.FileName);
            string resourceUrl = $"{Url.Action(Get())}?{MVC.Public.Resources.Actions.GetParams.resourceId}={id.ToString("N")}";
            return Content(resourceUrl);
        }
    }
}
