using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class DomainBreakdownBase<TEntity>
    {
        protected abstract IEnumerable<RelationdalDtoBase> BreakdownInternal(TEntity entity);

        protected IDtoFactory DtoFactory { get; private set; }

        public IEnumerable<RelationdalDtoBase> Breakdown(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            return BreakdownInternal(entity);
        }

        protected DomainBreakdownBase(IDtoFactory dtoFactory)
        {
            Shield.ArgumentNotNull(dtoFactory).ThrowOnError();

            DtoFactory = dtoFactory;
        }
    }
}
