using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class CultureInfoSetting : Setting
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
                _locale = value;
            }
        }

        public override string StringValue
        {
            get
            {
                return _locale?.Name;
            }

            set
            {
                _locale = new CultureInfo(value);
            }
        }

        protected CultureInfoSetting()
        {
        }

        public CultureInfoSetting(CultureInfo locale = null)
        {
            Locale = locale;
        }
    }
}
