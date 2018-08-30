using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using BGC.Utilities;
using BGC.Data.Relational.Mappings;
using System.Data.Entity;
using BGC.Core.Models;

namespace BGC.Data.Relational.Repositories
{
    internal class GlossaryRepository : EntityFrameworkRepository<Guid, GlossaryEntry, GlossaryEntryRelationalDto>, IGlossaryRepository
    {
        private readonly LambdaTypeSubstitution<IGlossaryEntryDto, GlossaryEntryRelationalDto> _expressionMap = new LambdaTypeSubstitution<IGlossaryEntryDto, GlossaryEntryRelationalDto>();

        public GlossaryRepository(GlossaryEntryTypeMapper typeMapper, DbContext ctx) :
            base(typeMapper, ctx)
        {
        }

        public IEnumerable<GlossaryEntry> Find(Expression<Func<IGlossaryEntryDto, bool>> selector)
        {
            Expression<Func<GlossaryEntryRelationalDto, bool>> actualDtoSelector = _expressionMap.ChangeLambdaType(selector);
            
            IEnumerable<GlossaryEntry> result = ExecuteAndMapQuery(DbContext.Set<GlossaryEntryRelationalDto>().Where(actualDtoSelector));
            return result;
        }
    }
}
