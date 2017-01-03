using BGC.Core.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Controllers
{
    public abstract class BgcControllerBase : Controller
    {
        [Dependency]
        public ILocalizationService LocalizationService { get; set; }

        private CultureInfo _currentLocale;
        public CultureInfo CurrentLocale
        {
            get
            {
                if (_currentLocale == null)
                {
                    string routeLocale = RouteData.Values["locale"]?.ToString();
                    _currentLocale = string.IsNullOrEmpty(routeLocale)
                        ? CultureInfo.GetCultureInfo("bg-BG")
                        : CultureInfo.GetCultureInfo(routeLocale);
                }

                return _currentLocale;
            }
        }

        public string Localize(string key) => LocalizationService?.Localize(key) ?? key;
    }
}