using BGC.Core.Services;
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
        private ILocalizationService localizationService;
        [Dependency]
        public ILocalizationService LocalizationService
        {
            get
            {
                return this.localizationService;
            }

            set
            {
                this.localizationService = value.ValueNotNull(nameof(LocalizationService)).GetValueOrThrow();
            }
        }

        public override void Execute()
        {
        }

        public string Localize(string key) => this.localizationService.Localize(key);

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