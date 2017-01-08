using MaxMind.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Models
{
    public class ContinentInfo : GeoLocation
    {
        public string Code { get; set; }

        [Constructor]
        public ContinentInfo(
            [Parameter("code")] string code = null,
            [Parameter("names")] Dictionary<string, string> names = null) :
            base(names: names)
        {
            Code = code;
        }
    }
}