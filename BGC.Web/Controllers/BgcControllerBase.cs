using BGC.Core;
using BGC.Core.Services;
using BGC.Utilities;
using BGC.Web.Attributes;
using BGC.Web.Models;
using BGC.Web.Services;
using CodeShield;
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
        private DependencyValue<CultureInfo> _currentLocale;

        [Dependency]
        public ILocalizationService LocalizationService { get; set; }

        private WebApplicationSettings _appProfile;
        [Dependency]
        public WebApplicationSettings ApplicationProfile
        {
            get
            {
                return _appProfile;
            }

            set
            {
                Shield.ValueNotNull(value).ThrowOnError();
                _appProfile = value;
            }
        }

        public virtual DependencyValue<CultureInfo> CurrentLocale
        {
            get
            {
                return (_currentLocale ?? (_currentLocale =
            RequestContextLocale.FromRequest(
                appProfile: ApplicationProfile,
                geoLocationService: DependencyResolver.Current.GetService<IGeoLocationService>(),
                request: Request,
                cookieStore: Request.Cookies[ApplicationProfile.LocaleCookieName])));
            }

            protected set
            {
                _currentLocale = value;
            }
        }

        public string Localize(string key) => LocalizationService?.Localize(key) ?? key;

        private string _defaultActionName;
        public virtual string DefaultActionName => _defaultActionName ?? (_defaultActionName = DefaultActionAttribute.GetDefaultActionName(GetType()));
    }
}