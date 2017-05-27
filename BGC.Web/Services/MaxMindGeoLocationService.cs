using BGC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using MaxMind.Db;
using CodeShield;

namespace BGC.Web.Services
{
    public class MaxMindGeoLocationService : IGeoLocationService
    {
        private readonly Reader _dbReader;

        public MaxMindGeoLocationService(Stream db)
        {
            _dbReader = new Reader(db.ArgumentNotNull().GetValueOrThrow());
        }

        public CountryInfo GetCountry(IPAddress ip) => _dbReader.Find<IpAddressInfo>(ip.ArgumentNotNull().GetValueOrThrow())?.Country;
    }
}