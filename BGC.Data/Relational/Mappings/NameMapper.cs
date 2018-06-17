using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Relational.Mappings
{
    internal class NameMapper : RelationalMapper<ComposerName, NameRelationalDto>
    {
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
