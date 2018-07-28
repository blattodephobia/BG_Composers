using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Exceptions
{
    public class ArticleNotFoundException : EntityNotFoundException
    {
        public ArticleNotFoundException(CultureInfo locale, Guid composerId) :
            base(locale, typeof(Composer), $"Composer '{composerId}' has no article associated with the locale {locale.Name}.")
        {

        }
    }
}
