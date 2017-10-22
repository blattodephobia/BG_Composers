using BGC.Utilities;
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
    public class MultiCultureInfoSetting : Setting, IParameter<IEnumerable<CultureInfo>>
    {
        private static readonly char[] Separators = new[] { ',', ' ', ';' };

        private IEnumerable<CultureInfo> _cultures;
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

                SetValue(ref _string, value);
                Cultures = value
                    .Split(Separators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => CultureInfo.GetCultureInfo(s));
            }
        }

        [NotMapped]
        public IEnumerable<CultureInfo> Cultures
        {
            get
            {
                return _cultures ?? Enumerable.Empty<CultureInfo>();
            }

            set
            {
                SetValue(ref _cultures, value);

                // generates a string of culture codes, separated by a comma and space, e.g. "en-US, de-DE"
                _string = _cultures?.ToStringAggregate(", ");
            }
        }

        protected MultiCultureInfoSetting() :
            base()
        {
        }

        public MultiCultureInfoSetting(string name) :
            base(name)
        {
        }

        public MultiCultureInfoSetting(string name, string culturesList = null) :
            base(name)
        {
            this.StringValue = culturesList.ArgumentNotNull();
        }

        IEnumerable<CultureInfo> IParameter<IEnumerable<CultureInfo>>.Value
        {
            get
            {
                return Cultures;
            }

            set
            {
                Cultures = value;
            }
        }
    }
}
