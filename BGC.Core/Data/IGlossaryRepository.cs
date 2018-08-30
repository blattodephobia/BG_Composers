using BGC.Core;
using BGC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data
{
    public interface IGlossaryRepository : INonQueryableRepository<Guid, GlossaryEntry>
    {
        IEnumerable<GlossaryEntry> Find(Expression<Func<IGlossaryEntryDto, bool>> selector);
    }
}
