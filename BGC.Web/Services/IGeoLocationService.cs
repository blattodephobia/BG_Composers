using BGC.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace BGC.Web.Services
{
    public interface IGeoLocationService
    {
        CountryInfo GetCountry(IPAddress ip);
    }
}