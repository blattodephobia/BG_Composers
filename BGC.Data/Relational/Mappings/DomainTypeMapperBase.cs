using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class DomainTypeMapperBase<TEntity, TDto>
        where TDto : RelationdalDtoBase
    {
        protected DomainTypeMapperBase(IDtoFactory dtoFactory)
        {
            Shield.ArgumentNotNull(dtoFactory).ThrowOnError();

            DtoFactory = dtoFactory;
        }

        protected abstract IEnumerable<RelationdalDtoBase> BreakdownInternal(TEntity entity);

        protected IDtoFactory DtoFactory { get; private set; }

        protected abstract TEntity BuildInternal(TDto dto);

        public TEntity Build(TDto dto)
        {
            Shield.ArgumentNotNull(dto).ThrowOnError();

            return BuildInternal(dto);
        }

        public IEnumerable<RelationdalDtoBase> Breakdown(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            return BreakdownInternal(entity);
        }
    }
}
