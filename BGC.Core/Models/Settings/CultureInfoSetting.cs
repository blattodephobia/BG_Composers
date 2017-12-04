using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class CultureInfoSetting : Setting, IParameter<CultureInfo>
    {
        private CultureInfo _locale;
        [NotMapped]
        public CultureInfo Locale
        {
            get
            {
                return _locale;
            }

            set
            {
                SetValue(ref _locale, value);
            }
        }

        public override Type ValueType => typeof(CultureInfo);

        public override string StringValue
        {
            get
            {
                return _locale?.Name;
            }

            set
            {
                SetValue(ref _locale, new CultureInfo(value));
            }
        }

        CultureInfo IParameter<CultureInfo>.Value
        {
            get
            {
                return Locale;
            }

            set
            {
                Locale = value;
            }
        }

        protected CultureInfoSetting() :
            base()
        {
        }

        public CultureInfoSetting(string name, CultureInfo locale = null) :
            base(name)
        {
            Locale = locale;
        }
    }
}
