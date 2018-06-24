using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace BGC.Data.Relational.Mappings
{
    // for the moment, there are no simple properties to be transferred from the entity to the DTO; that's why the mapper is virtually empty
    internal class ProfilePropertyMapper : RelationalPropertyMapper<ComposerProfile, ProfileRelationalDto>
    {
        protected override void CopyDataInternal(ComposerProfile source, ProfileRelationalDto target)
        {
        }

        protected override void CopyDataInternal(ProfileRelationalDto source, ComposerProfile target)
        {
        }

        protected override Expression<Func<ProfileRelationalDto, bool>> GetComparisonInternal(ComposerProfile entity) => (dto) => dto.Id == entity.Id;
    }
}
