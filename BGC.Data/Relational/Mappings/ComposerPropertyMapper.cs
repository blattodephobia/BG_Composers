using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerPropertyMapper : RelationalPropertyMapper<Composer, ComposerRelationalDto>
    {
        protected override Expression<Func<ComposerRelationalDto, bool>> GetComparisonInternal(Composer entity) => (dto) => dto.Id == entity.Id;

        protected override void CopyDataInternal(Composer source, ComposerRelationalDto target)
        {
            Shield.ArgumentNotNull(source).ThrowOnError();
            Shield.ArgumentNotNull(target).ThrowOnError();

            target.Id = source.Id;
            target.DateOfBirth = source.DateOfBirth;
            target.DateOfDeath = source.DateOfDeath;
            target.Order = source.Order;
        }

        protected override void CopyDataInternal(ComposerRelationalDto source, Composer target)
        {
            target.Id = source.Id;
            target.DateOfBirth = source.DateOfBirth;
            target.DateOfDeath = source.DateOfDeath;
            target.Order = source.Order;
        }
    }
}
