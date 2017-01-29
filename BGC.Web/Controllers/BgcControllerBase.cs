using BGC.Core.Services;
using BGC.Web.Attributes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Controllers
{
    public abstract class BgcControllerBase : Controller
    {
        [Dependency]
        public ILocalizationService LocalizationService { get; set; }

        private CultureInfo _currentLocale;
        public virtual CultureInfo CurrentLocale
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

        private string _defaultActionName;
        public virtual string DefaultActionName => _defaultActionName ?? (_defaultActionName = DefaultActionAttribute.GetDefaultActionName(GetType()));
    }
}