using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal interface IDtoFactory
    {
        TRelationalDto GetDtoFor<TRelationalDto, TEntity>(TEntity entity, RelationalPropertyMapper<TEntity, TRelationalDto> keyMapper) where TRelationalDto : RelationdalDtoBase;
    }
}
