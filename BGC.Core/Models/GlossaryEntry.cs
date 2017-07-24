using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public class GlossaryEntry : BgcEntity<Guid>
    {
        private ICollection<GlossaryDefinition> _defiinitions;

        public ICollection<GlossaryDefinition> Definitions
        {
            get
            {
                return _defiinitions ?? (_defiinitions = new HashSet<GlossaryDefinition>());
            }

            set
            {
                _defiinitions = value;
            }
        }

        public GlossaryEntry()
        {

        }
    }
}
