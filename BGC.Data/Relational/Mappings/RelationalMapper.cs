using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class RelationalMapper<TKey, TEntity, TDto>
        where TKey : struct
        where TEntity : BgcEntity<TKey>
        where TDto : RelationdalDtoBase
    {
        public abstract TEntity ToEntity(TDto dto);

        public abstract void CopyData(TEntity source, TDto target);
    }
}
