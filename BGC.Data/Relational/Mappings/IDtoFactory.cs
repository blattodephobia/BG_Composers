using BGC.Data.Relational.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational
{
    internal interface IDtoFactory
    {
        /// <summary>
        /// Returns an object of type <typeparamref name="TRelationalDto"/> corresponding to the <paramref name="entity"/>. The returned object may or may not have all of its properties set.
        /// </summary>
        /// <typeparam name="TRelationalDto"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">The entity whose DTO should be created or looked up. Can be null.</param>
        /// <param name="mapper"></param>
        /// <returns></returns>
        TRelationalDto GetDtoFor<TRelationalDto, TEntity>(TEntity entity, IPropertyMapper<TEntity, TRelationalDto> mapper) where TRelationalDto : RelationdalDtoBase;
    }
}
