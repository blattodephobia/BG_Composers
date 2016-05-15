using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class DateTimeParameter : StringParameter
    {
        public static readonly string Format = @"dd MMM yyyy, hh:mm:ss.fff";
        public static readonly CultureInfo Culture = CultureInfo.GetCultureInfo("en-US");

        public override string StringValue
        {
            get
            {
                return Date.ToString(Format, Culture);
            }

            set
            {
                Date = DateTime.Parse(Format, Culture);
            }
        }

        public DateTime Date { get; set; }
    }
}
