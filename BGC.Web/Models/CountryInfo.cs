using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Models
{
    public class CountryInfo : GeoLocation
    {
        private static readonly Dictionary<string, HashSet<CultureInfo>> CountryCultures =
            (from culture in CultureInfo.GetCultures(CultureTypes.AllCultures)
             let country = culture.Name.Substring(culture.Name.Length - 2, 2)
             where country.ToUpperInvariant() == country
             group culture by country into countryInfo
             select countryInfo).ToDictionary(c => c.Key, c => new HashSet<CultureInfo>(c));

        private IEnumerable<CultureInfo> _localCultures;
        public IEnumerable<CultureInfo> LocalCultures
        {
            get
            {
                if (_localCultures == null)
                {
                    _localCultures = IsoCode != null && CountryCultures.ContainsKey(IsoCode)
                        ? CountryCultures[IsoCode]
                        : Enumerable.Empty<CultureInfo>();
                }

                return _localCultures;
            }
        }

        public string IsoCode { get; private set; }

        [Constructor]
        public CountryInfo(
            [Parameter("iso_code")] string isoCode = null,
            [Parameter("names")] Dictionary<string, string> names = null) :
            base(names: names)
        {
            IsoCode = isoCode;
        }
    }
}