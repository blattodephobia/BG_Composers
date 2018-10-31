using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal interface IPropertyMapper<TEntity, TDto>
        where TDto : RelationdalDtoBase
    {
        TDto CopyData(TEntity source, TDto target);

        TEntity CopyData(TDto source, TEntity target);

        /// <summary>
        /// Builds an <see cref="Expression"/> that can uniquely identify a <see cref="TDto"/> instance as corresponding to the <paramref name="entity"/> or not.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Expression<Func<TDto, bool>> GetKeyPredicateFor(TEntity entity);
    }
}
