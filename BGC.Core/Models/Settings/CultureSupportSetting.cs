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
    public class CultureSupportSetting : Setting, IParameter<IEnumerable<CultureInfo>>
    {
        private static readonly char[] Separators = new[] { ',', ' ', ';' };
        private static readonly string AppendSeparator = ", ";

        private IEnumerable<CultureInfo> supportedCultures;
        private string _string;

        public override string StringValue
        {
            get
            {
                return this._string;
            }

            set
            {
                Shield.ValueNotNull(value, nameof(StringValue)).ThrowOnError();
                
                SupportedCultures = value
                    .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => CultureInfo.GetCultureInfo(s));
                
                this._string = value;
            }
        }

        [NotMapped]
        public IEnumerable<CultureInfo> SupportedCultures
        {
            get
            {
                return this.supportedCultures;
            }

            set
            {
                this.supportedCultures = value;

                // generates a string of culture codes, separated by a comma and space, e.g. "en-US, de-DE"
                this._string = this.supportedCultures?
                    .Aggregate(
                        new StringBuilder(),
                        (sb, current) => sb.AppendFormat("{0}{1}", current.Name, AppendSeparator),
                        sb => sb.Length >= AppendSeparator.Length
                            ? sb.Remove(sb.Length - AppendSeparator.Length, AppendSeparator.Length)
                            : sb)
                    .ToString();
            }
        }

        public CultureSupportSetting()
        {
        }

        public CultureSupportSetting(string culturesList) :
            this()
        {
            this.StringValue = culturesList.ArgumentNotNull().GetValueOrThrow();
        }

        IEnumerable<CultureInfo> IParameter<IEnumerable<CultureInfo>>.Value
        {
            get
            {
                return SupportedCultures;
            }

            set
            {
                SupportedCultures = value;
            }
        }
    }
}
