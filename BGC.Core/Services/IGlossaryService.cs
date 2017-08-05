using BGC.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public interface IGlossaryService
    {
        IEnumerable<GlossaryEntry> ListAll();

        void AddOrUpdate(GlossaryEntry entry);

        void Delete(GlossaryEntry entry);

        GlossaryEntry Find(Guid id);
    }
}
