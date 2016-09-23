using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class BgcRole : IdentityRole<long, BgcUserRole>
	{
		public BgcRole()
		{
		}

		public BgcRole(string roleName)
		{
			Name = roleName;
        }

        /// <summary>
        /// Determines whether this role can be deleted from the data store or not; crucial, compile-time known roles cannot be deleted.
        /// </summary>
        public virtual bool CanDelete => true;

        public virtual ICollection<Permission> Permissions { get; set; }

        public TPermission GetPermission<TPermission>()
            where TPermission : Permission, new()
        {
            return Permissions?.Where(p => p.GetType() == typeof(TPermission)).SingleOrDefault() as TPermission;
        }
    }
}
