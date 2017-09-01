using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public class GlossaryEntry : BgcEntity<Guid>
    {
        private ICollection<GlossaryDefinition> _definitions;

        public virtual ICollection<GlossaryDefinition> Definitions
        {
            get
            {
                return _definitions ?? (_definitions = new HashSet<GlossaryDefinition>());
            }

            set
            {
                _definitions = value;
            }
        }

        public GlossaryDefinition GetDefinition(CultureInfo locale)
        {
            Shield.ArgumentNotNull(locale, nameof(locale)).ThrowOnError();

            return Definitions.FirstOrDefault(d => d.Language.Equals(locale));
        }
    }
}
