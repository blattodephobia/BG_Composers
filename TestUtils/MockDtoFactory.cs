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

        IIntermediateRelationalDto<TPrinicpalDto, TDependantDto> IDtoFactory.GetIntermediateDto<TPrinicpalDto, TDependantDto>(TPrinicpalDto principal, TDependantDto dependantEntity)
        {
            return (IIntermediateRelationalDto<TPrinicpalDto, TDependantDto>)IntermediateDtoCallback?.Invoke(principal, dependantEntity);
        }

        public virtual object ActivateObject(Type type)
        {
            return Activator.CreateInstance(type, nonPublic: true);
        }

        public Func<object, object, object> IntermediateDtoCallback { get; set; }
    }
}
