using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class ComposerMapper : RelationalMapper<Composer, ComposerRelationalDto>
    {
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
