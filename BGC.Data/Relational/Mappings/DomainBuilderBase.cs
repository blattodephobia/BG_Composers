using BGC.Data.Relational.ManyToMany;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal abstract class DomainBuilderBase<TDto, TEntity>
        where TDto : RelationdalDtoBase, INavigationalDto
    {
        public abstract TEntity Build(TDto dto);
    }
}
