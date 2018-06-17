using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    // for the moment, there are no simple properties to be transferred from the entity to the DTO; that's why the mapper is virtually empty
    internal class ProfileMapper : RelationalMapper<ComposerProfile, ProfileRelationalDto>
    {
        protected override void CopyDataInternal(ComposerProfile source, ProfileRelationalDto target)
        {
        }

        protected override void CopyDataInternal(ProfileRelationalDto source, ComposerProfile target)
        {
        }
    }
}
