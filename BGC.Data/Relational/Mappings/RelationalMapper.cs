using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class RelationalMapper<TEntity, TDto>
        where TDto : RelationdalDtoBase
    {
        public abstract TEntity ToEntity(TDto dto);

        public abstract TDto CopyData(TEntity source, TDto target);
    }
}
