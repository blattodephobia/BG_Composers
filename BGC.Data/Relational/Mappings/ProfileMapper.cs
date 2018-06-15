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
        public override ProfileRelationalDto CopyData(ComposerProfile source, ProfileRelationalDto target)
        {
            return target;
        }

        public override ComposerProfile ToEntity(ProfileRelationalDto dto)
        {
            return new ComposerProfile();
        }
    }
}
