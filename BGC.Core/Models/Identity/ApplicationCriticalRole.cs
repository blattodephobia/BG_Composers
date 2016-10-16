using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class ApplicationCriticalRole : BgcRole
    {
        public sealed override bool CanDelete => false;

        internal ApplicationCriticalRole()
        {
        }
    }
}
