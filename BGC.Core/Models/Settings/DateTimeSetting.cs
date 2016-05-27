using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class DateTimeSetting : Setting
    {
        /// <summary>
        /// The format used during string conversion for the <see cref="StringValue"/> property.
        /// It's set to "dd MMM yyyy, HH:mm:ss.fff"
        /// </summary>
        public static readonly string Format = @"dd MMM yyyy, HH:mm:ss.fff";

        /// <summary>
        /// The culture used during string conversion for the <see cref="StringValue"/> property.
        /// It's set to en-US.
        /// </summary>
        public static readonly IFormatProvider FormatProvider = CultureInfo.GetCultureInfo("en-US");

        public override string StringValue
        {
            get
            {
                return Date.ToString(Format, FormatProvider);
            }

            set
            {
                string _value = value.IsNotNullOrEmpty(nameof(StringValue)).GetValueOrThrow();
                Date = DateTime.Parse(_value, FormatProvider);
            }
        }
        
        public DateTime Date { get; set; }
    }
}
