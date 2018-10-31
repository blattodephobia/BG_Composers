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
    internal abstract class DomainTypeMapperBase<TEntity, TDto> : ITypeMapper<TEntity, TDto>
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

        protected abstract TDto BuildDtoInternal(TEntity entity);

        public TEntity Build(TDto dto)
        {
            Shield.ArgumentNotNull(dto).ThrowOnError();

            return BuildInternal(dto);
        }

        TEntity ITypeMapper<TEntity, TDto>.BuildEntity(TDto dto) => Build(dto);

        public TDto BuildDto(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            return BuildDtoInternal(entity);
        }

        [Obsolete(nameof(Breakdown) + " is deprecated. Use " + nameof(BuildDto) + " instead.")]
        public IEnumerable<RelationdalDtoBase> Breakdown(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            return BreakdownInternal(entity);
        }
    }
}
