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
        public override ComposerRelationalDto CopyData(Composer source, ComposerRelationalDto target)
        {
            Shield.ArgumentNotNull(source).ThrowOnError();
            Shield.ArgumentNotNull(target).ThrowOnError();

            target.Id = source.Id;
            target.DateOfBirth = source.DateOfBirth;
            target.DateOfDeath = source.DateOfDeath;
            target.Order = source.Order;

            return target;
        }

        public override Composer ToEntity(ComposerRelationalDto dto)
        {
            Shield.ArgumentNotNull(dto).ThrowOnError();

            return new Composer()
            {
                Id = dto.Id,
                DateOfBirth = dto.DateOfBirth,
                DateOfDeath = dto.DateOfDeath,
                Order = dto.Order,
            };
        }
    }
}
