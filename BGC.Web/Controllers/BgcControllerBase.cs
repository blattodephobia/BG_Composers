using BGC.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Controllers
{
    public abstract class BgcControllerBase : Controller
    {
        [Dependency]
        public ILocalizationService LocalizationService { get; set; }

        public string Localize(string key) => LocalizationService?.Localize(key) ?? key;
    }
}