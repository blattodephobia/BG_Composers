using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class DomainBreakdownBase<TEntity>
    {
        public abstract IEnumerable<RelationdalDtoBase> Breakdown(TEntity entity);
    }
}
