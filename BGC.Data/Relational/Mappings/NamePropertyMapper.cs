using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BGC.Data.Relational.Mappings
{
    internal class NamePropertyMapper : RelationalPropertyMapper<ComposerName, NameRelationalDto>
    {
        protected override Expression<Func<NameRelationalDto, bool>> GetComparisonInternal(ComposerName entity) => (dto) => dto.FullName == entity.FullName && dto.Composer_Id == entity.Composer.Id;

        protected override void CopyDataInternal(ComposerName source, NameRelationalDto target)
        {
            target.FullName = source.FullName;
            target.Id = source.Id;
            target.Language = source.Language.Name;
        }

        protected override void CopyDataInternal(NameRelationalDto source, ComposerName target)
        {
            target.Id = source.Id;
        }
    }
}
