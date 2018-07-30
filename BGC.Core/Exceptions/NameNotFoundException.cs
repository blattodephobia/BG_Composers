using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Exceptions
{
    public class NameNotFoundException : EntityNotFoundException
    {
        public NameNotFoundException(CultureInfo locale, Guid composerId) :
            base(locale, typeof(Composer), $"Composer '{composerId}' has no localized name in the {locale.Name} locale.")
        {

        }
    }
}
