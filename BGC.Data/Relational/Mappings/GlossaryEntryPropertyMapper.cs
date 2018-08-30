using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BGC.Core.Models;

namespace BGC.Data.Relational.Mappings
{
    internal class GlossaryEntryPropertyMapper : RelationalPropertyMapper<GlossaryEntry, GlossaryEntryRelationalDto>
    {
        protected override void CopyDataInternal(GlossaryEntry source, GlossaryEntryRelationalDto target)
        {
            target.DomainId = source.Id;
        }

        protected override void CopyDataInternal(GlossaryEntryRelationalDto source, GlossaryEntry target)
        {
            target.Id = source.DomainId;
        }

        protected override Expression<Func<GlossaryEntryRelationalDto, bool>> GetComparisonInternal(GlossaryEntry entity)
        {
            return (GlossaryEntryRelationalDto dto) => dto.DomainId == entity.Id;
        }
    }
}
