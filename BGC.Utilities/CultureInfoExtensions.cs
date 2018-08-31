using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Globalization
{
    public static class CultureInfoExtensions
    {
        public static CultureInfo ToCultureInfo(this string languageCode) => CultureInfo.GetCultureInfo(languageCode);
    }
}
