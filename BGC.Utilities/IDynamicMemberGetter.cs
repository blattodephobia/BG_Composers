using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    internal interface IDynamicMemberGetter
    {
        object GetMemberValue(object obj, string memberName);
    }
}
