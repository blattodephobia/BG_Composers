using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    /// <summary>
    /// The role of all registered users. Doesn't contain special permissions.
    /// </summary>
    public class AuthorizedUserRole : BgcRole
    {
        public override bool CanDelete => false;
    }
}
