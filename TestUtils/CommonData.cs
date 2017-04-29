using BGC.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public static class CommonData
    {
        public static ApplicationProfile GetCommonApplicationProfile()
        {
            ApplicationProfile result = ApplicationProfile.FromApplicationSettings(new[]
            {
                new CultureSupportSetting()
                {
                    Name = nameof(ApplicationProfile.SupportedLanguages), SupportedCultures = new[]
                    {
                        CultureInfo.GetCultureInfo("en-US"), CultureInfo.GetCultureInfo("de-DE")
                    }
                }
            });

            result.LocaleCookieName = "localeCookie";
            result.LocaleRouteTokenName = "localRouteToken";

            return result;
        }
    }
}
