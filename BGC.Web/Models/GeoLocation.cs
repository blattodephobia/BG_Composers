using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Models
{
    public class GeoLocation
    {
        public Dictionary<string, string> Names { get; set; }

        [Constructor]
        public GeoLocation(
            [Parameter("names")] Dictionary<string, string> names = null)
        {
            Names = names ?? new Dictionary<string, string>();
        }
    }
}