using BGC.Data.Relational;
using BGC.Data.Relational.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public class MockDtoFactory : IDtoFactory
    {
        TRelationalDto IDtoFactory.GetDtoFor<TRelationalDto, TEntity>(TEntity entity, RelationalPropertyMapper<TEntity, TRelationalDto> keyMapper)
        {
            return ActivateObject(typeof(TRelationalDto)) as TRelationalDto;
        }

        public virtual object ActivateObject(Type type)
        {
            return Activator.CreateInstance(type, nonPublic: true);
        }
    }
}
