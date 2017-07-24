using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class GlossaryManagementPermission : Permission, IGlossaryManagementPermission
    {
        public override string Name => nameof(IGlossaryManagementPermission);
    }
}
