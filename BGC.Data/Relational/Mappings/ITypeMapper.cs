using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal interface ITypeMapper<TEntity, TDto> where TDto : RelationdalDtoBase
    {
        TEntity BuildEntity(TDto dto);

        TDto BuildDto(TEntity entity);
    }
}
