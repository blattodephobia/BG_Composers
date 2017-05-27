using BGC.Core;
using BGC.Utilities;
using BGC.Web.Models;
using BGC.Web.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;
using static BGC.Web.WebApiApplication;

namespace BGC.Web.Models
{
    public class RequestContextLocale : DependencyValue<CultureInfo>
    {
        private static readonly TypeConverter CultureConverter = new CultureInfoConverter();

        private static IPAddress GetIpFromRequest(HttpRequestBase context)
        {
            try
            {
                byte[] ipAddressBytes = context.UserHostAddress.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToByte(s)).ToArray();
                return new IPAddress(ipAddressBytes);
            }
            catch
            {
                return null;
            }
        }

        public static RequestContextLocale FromRequest(ApplicationProfile appProfile, IGeoLocationService geoLocationService, HttpRequestBase request, HttpCookie cookieStore)
        {
            Shield.ArgumentNotNull(request).ThrowOnError();
            Shield.ArgumentNotNull(appProfile).ThrowOnError();
            Shield.ArgumentNotNull(geoLocationService).ThrowOnError();

            RequestContextLocale result = new RequestContextLocale(appProfile, cookieStore);
            IPAddress ip = GetIpFromRequest(request);
            IEnumerable<CultureInfo> validIpLocales = ip != null
                ? (geoLocationService.GetCountry(ip)?.LocalCultures ?? Enumerable.Empty<CultureInfo>()).Select(c => result.CoerceValue(c))
                : Enumerable.Empty<CultureInfo>();
            result.ValidIpLocales.SetValueRange(validIpLocales);

            CultureInfo routeLocale = result.GetValidCultureInfoOrDefault(request.RequestContext.RouteData.Values[appProfile.LocaleRouteTokenName]?.ToString());
            result.ValidRouteLocale.SetValue(routeLocale);

            return result;
        }

        private HashSet<CultureInfo> _supportedCultures;

        private CultureInfo GetValidCultureInfoOrDefault(string cultureCode)
        {
            try
            {
                return CoerceValue(CultureInfo.GetCultureInfo(cultureCode));
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        protected override CultureInfo CoerceValue(CultureInfo culture)
        {
            return culture != null
                ? _supportedCultures.Contains(culture) ? culture : null
                : null;
        }

        [DependencyPrecedence(0)]
        public MultiValueFifoDependencySource<CultureInfo> ValidIpLocales { get; private set; } = new MultiValueFifoDependencySource<CultureInfo>();
        
        [DependencyPrecedence(1)]
        public HttpCookieSingleValueDependencySource<CultureInfo> CookieLocale { get; private set; }

        [DependencyPrecedence(2)]
        public SingleValueDependencySource<CultureInfo> ValidRouteLocale { get; private set; } = new SingleValueDependencySource<CultureInfo>(false);

        /// <summary>
        /// Converts the <param name="source"></param> to a valid <see cref="CultureInfo"/> if that locale is supported by .NET or to null.
        /// The converted value is then set on <param name="source"></param> source.
        /// </summary>
        /// <param name="cultureCode"></param>
        public void SetValidLocaleOrDefault(DependencySource<CultureInfo> source, string cultureCode)
        {
            Shield.ArgumentNotNull(source).ThrowOnError();

            CultureInfo culture = null;
            try
            {
                culture = new CultureInfo(cultureCode);
            }
            catch (CultureNotFoundException)
            {
            }
            catch (ArgumentNullException)
            {
            }

            source.SetValue(culture);
        }
        
        public RequestContextLocale(ApplicationProfile appProfile, HttpCookie cookie) :
            base(appProfile?.SupportedLanguages?.FirstOrDefault())
        {
            Shield.ArgumentNotNull(appProfile).ThrowOnError();
            Shield.ArgumentNotNull(cookie).ThrowOnError();
            Shield.IsNotNullOrEmpty(appProfile.SupportedLanguages).ThrowOnError();

            CookieLocale = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, appProfile.LocaleKey, CultureConverter);
            _supportedCultures = new HashSet<CultureInfo>(appProfile.SupportedLanguages);
        }
    }
}