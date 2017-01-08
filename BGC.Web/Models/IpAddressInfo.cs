using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Models
{
    public class IpAddressInfo
    {
        public ContinentInfo Continent { get; set; }
        public CountryInfo Country { get; set; }
        public CountryInfo RegisteredCountry { get; set; }

        [Constructor]
        public IpAddressInfo(
            [Parameter("continent")] ContinentInfo continent = null,
            [Parameter("country")] CountryInfo country = null,
            [Parameter("registered_country")] CountryInfo registeredCountry = null)
        {
            Continent = continent;
            Country = country;
            RegisteredCountry = registeredCountry;
        }
    }
}