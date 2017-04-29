using BGC.Core;
using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.Models
{
    public class UserLocaleDependencyValue : DependencyValue<CultureInfo>
    {
        private static readonly CultureInfoConverter CultureConverter = new CultureInfoConverter();

        private readonly IEnumerable<CultureInfo> _supportedCultures;

        protected override CultureInfo CoerceValue(CultureInfo value)
        {
            return value != null && _supportedCultures.Contains(value)
                ? value
                : null;
        }

        /// <summary>
        /// This property is for when the <see cref="DbSetting"/> isn't present in the user's setting or the user hasn't logged in yet.
        /// </summary>
        [DependencyPrecedence(0)]
        public SingleValueDependencySource<CultureInfo> CookieSetting { get; private set; }
        
        [DependencyPrecedence(1)]
        public SingleValueDependencySource<CultureInfo> DbSetting { get; private set; } = new SingleValueDependencySource<CultureInfo>(false);

        public UserLocaleDependencyValue(ApplicationProfile appProfile, HttpCookie cookieStore) :
            base(appProfile?.SupportedLanguages?.First())
        {
            Shield.ArgumentNotNull(appProfile).ThrowOnError();
            Shield.ArgumentNotNull(cookieStore).ThrowOnError();

            try
            {
                _supportedCultures = appProfile.SupportedLanguages;
                CookieSetting = new HttpCookieSingleValueDependencySource<CultureInfo>(cookieStore, appProfile.LocaleCookieName, CultureConverter);
            }
            catch (ArgumentNullException)
            {
            }
            catch (CultureNotFoundException)
            {
            }
        }
    }
}