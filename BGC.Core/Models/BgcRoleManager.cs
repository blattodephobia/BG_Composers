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
        public BgcRoleManager(RoleStore<BgcRole, long, BgcUserRole> store) :
            base(store)
        {
        }

        /// <summary>
        /// Returns an object that derives from <see cref="BgcRole"/> that corresponds to the name of the role
        /// stored in the <paramref name="dbEntity"/> parameter's <see cref="IdentityRole{long, BgcUserRole}.Name"/> property, if
        /// such a type exists.
        /// </summary>
        /// <param name="dbEntity"></param>
        /// <returns></returns>
        public BgcRole GetDerivedRole(BgcRole dbEntity)
        {
            return typeof(BgcRole).Assembly.GetExportedTypes().Where(t => typeof(BgcRole).IsAssignableFrom(t) && dbEntity.Name == t.Name).FirstOrDefault();
        }
    }
}
