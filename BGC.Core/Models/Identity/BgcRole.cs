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

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
