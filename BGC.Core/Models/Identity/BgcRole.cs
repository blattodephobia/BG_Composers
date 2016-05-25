using BGC.Core.Models;
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
		public static readonly string AdministratorRoleName = "Administrator";

		public BgcRole() :
			this(string.Empty)
		{

		}

		public BgcRole(string roleName)
		{
			this.Name = roleName;
        }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
