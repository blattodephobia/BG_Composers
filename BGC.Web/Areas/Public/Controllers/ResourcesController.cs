using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public.Controllers
{
    public class ResourcesController : Controller
    {
        private IDataStorageService textStorageService;

        public ResourcesController(IDataStorageService textStorageService)
        {
            this.textStorageService = textStorageService.ArgumentNotNull(nameof(textStorageService)).GetValueOrThrow();
        }

        public string GetText(Guid id)
        {
            return this.textStorageService.GetEntry(id);
        }
    }
}
