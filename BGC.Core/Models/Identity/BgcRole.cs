using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public abstract class BgcRole : IdentityRole<long, BgcUserRole>
	{
		public static readonly string AdministratorRoleName = "Administrator";

		protected BgcRole() :
			this(string.Empty)
		{
		}

		protected BgcRole(string roleName)
		{
			this.Name = roleName;
        }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
