using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class RelationalPropertyMapper<TEntity, TDto> : IPropertyMapper<TEntity, TDto>
        where TDto : RelationdalDtoBase
    {
        protected abstract void CopyDataInternal(TEntity source, TDto target);

        protected abstract void CopyDataInternal(TDto source, TEntity target);

        protected abstract Expression<Func<TDto, bool>> GetComparisonInternal(TEntity entity);

        public Expression<Func<TDto, bool>> GetKeyPredicateFor(TEntity entity)
        {
            Shield.ArgumentNotNull(entity).ThrowOnError();

            return GetComparisonInternal(entity);
        }

        /// <summary>
        /// Copies the properties of <paramref name="source"/> or maps them to <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The entity source. This value can be null. If it's null, no copying will take place.</param>
        /// <param name="target">The target relational DTO. If <paramref name="source"/> is not null,
        /// but <paramref name="target"/> is, a new instance of <typeparamref name="TDto"/> will be created.</param>
        /// <returns>The instance of <typeparamref name="TDto"/> that was passed or instantiated.</returns>
        public TDto CopyData(TEntity source, TDto target)
        {
            if (source != null && target != null)
            {
                CopyDataInternal(source, target);
            }

            return target;
        }

        public TEntity CopyData(TDto source, TEntity target)
        {
            Shield.ArgumentNotNull(source).ThrowOnError();
            Shield.ArgumentNotNull(target).ThrowOnError();

            CopyDataInternal(source, target);

            return target;
        }
    }
}
