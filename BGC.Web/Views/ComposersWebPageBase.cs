using BGC.Core.Services;
using BGC.Web.Controllers;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Views
{
    public class ComposersWebPageBase<T> : WebViewPage<T>
    {
        private ILocalizationService _localizationService;
        [Dependency]
        public ILocalizationService LocalizationService
        {
            get
            {
                // Instantiating the localization service like this is an anti-pattern.
                // However, Unity cannot inject the property when the current web page is a layout page - which is not supported
                // in ASP.NET MVC 3 by design (great idea, Microsoft). For the moment, the service will be instantiated like this
                // when it's null. For normal pages DI works as expected. 
                return _localizationService ?? (_localizationService = DependencyResolver.Current.GetService<ILocalizationService>());
            }

            set
            {
                _localizationService = value.ValueNotNull(nameof(LocalizationService)).GetValueOrThrow();
            }
        }

        public override void Execute()
        {
        }

        public string Localize(string key) => LocalizationService.Localize(key, (ViewContext.Controller as BgcControllerBase).CurrentLocale);

        public bool IsDebugBuild
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }
    }

    public class ComposersWebPageBase : ComposersWebPageBase<object>
    {
    }
}