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

        public static RequestContextLocale FromRequest(IEnumerable<CultureInfo> supportedCultures, IGeoLocationService geoLocationService, HttpRequestBase request, HttpCookie cookie)
        {
            Shield.ArgumentNotNull(request).ThrowOnError();
            Shield.ArgumentNotNull(supportedCultures).ThrowOnError();
            Shield.ArgumentNotNull(geoLocationService).ThrowOnError();

            RequestContextLocale result = FromCookie(supportedCultures, cookie);
            result.CookieLocale.SetValue(result.GetValidCultureInfoOrDefault(request.Cookies[LocaleCookieName]?.Values[LocaleRouteTokenName]));
            IPAddress ip = GetIpFromRequest(request);
            IEnumerable<CultureInfo> validIpLocales = ip != null
                ? geoLocationService.GetCountry(ip).LocalCultures.Select(c => result.CoerceValue(c))
                : Enumerable.Empty<CultureInfo>();
            result.ValidIpLocales.SetValueRange(validIpLocales);

            CultureInfo routeLocale = result.GetValidCultureInfoOrDefault(request.RequestContext.RouteData.Values[LocaleRouteTokenName]?.ToString());
            result.ValidRouteLocale.SetValue(routeLocale);

            return result;
        }

        public static RequestContextLocale FromCookie(IEnumerable<CultureInfo> supportedCultures, HttpCookie cookie)
        {
            Shield.ArgumentNotNull(supportedCultures).ThrowOnError();
            Shield.ArgumentNotNull(cookie).ThrowOnError();

            RequestContextLocale result = new RequestContextLocale(supportedCultures);
            result.CookieLocale = new HttpCookieSingleValueDependencySource<CultureInfo>(cookie, LocaleRouteTokenName, CultureConverter);
            result._supportedCultures = new HashSet<CultureInfo>(supportedCultures);

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

        public RequestContextLocale(IEnumerable<CultureInfo> supportedCultures) :
            base(supportedCultures?.FirstOrDefault())
        {
            Shield.ArgumentNotNull(supportedCultures);

            _supportedCultures = new HashSet<CultureInfo>(supportedCultures);
        }
    }
}