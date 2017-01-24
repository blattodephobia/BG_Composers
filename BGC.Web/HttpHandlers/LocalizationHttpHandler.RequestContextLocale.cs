using BGC.Utilities;
using BGC.Web.Models;
using BGC.Web.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Routing;

namespace BGC.Web.HttpHandlers
{
    public partial class LocalizationHttpHandler
    {
        public class RequestContextLocale : DependencyValue<CultureInfo>
        {
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

            private readonly HashSet<CultureInfo> _supportedCultures;
            private readonly IGeoLocationService _locationService;

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
            public SingleValueDependencySource<CultureInfo> CookieLocale { get; private set; } = new SingleValueDependencySource<CultureInfo>(false);

            [DependencyPrecedence(2)]
            public SingleValueDependencySource<CultureInfo> ValidRouteLocale { get; private set; } = new SingleValueDependencySource<CultureInfo>(false);

            protected RequestContextLocale(IEnumerable<CultureInfo> supportedCultures, IGeoLocationService geoLocationService) :
                base(Shield.ArgumentNotNull(supportedCultures).GetValueOrThrow().First())
            {
                Shield.ArgumentNotNull(geoLocationService).ThrowOnError();

                _supportedCultures = new HashSet<CultureInfo>(supportedCultures);
                _locationService = geoLocationService;
            }

            public RequestContextLocale(HttpRequestBase request, IEnumerable<CultureInfo> supportedCultures, IGeoLocationService geoLocationService) :
                    this(supportedCultures, geoLocationService)
            {
                Shield.ArgumentNotNull(request).ThrowOnError();
                Shield.ArgumentNotNull(supportedCultures).ThrowOnError();

                CookieLocale.SetValue(GetValidCultureInfoOrDefault(request.Cookies[LocaleCookieName]?.Values[LocaleRouteTokenName]));
                IPAddress ip = GetIpFromRequest(request);
                IEnumerable<CultureInfo> validIpLocales = ip != null
                    ? _locationService.GetCountry(ip).LocalCultures.Select(c => CoerceValue(c))
                    : Enumerable.Empty<CultureInfo>();
                ValidIpLocales.SetValueRange(validIpLocales);

                CultureInfo routeLocale = GetValidCultureInfoOrDefault(request.RequestContext.RouteData.Values[LocaleRouteTokenName]?.ToString());
                ValidRouteLocale.SetValue(routeLocale);
            }
        }
    }
}