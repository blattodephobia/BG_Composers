using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class BgcRoleManager : RoleManager<BgcRole, long>
    {
        public BgcRoleManager(IRoleStore<BgcRole, long> store) :
            base(store)
        {
        }
    }
}
