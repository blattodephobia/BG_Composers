using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class AspNetRole : IdentityRole<long, AspNetUserRole>
	{
		public static readonly string AdministratorRoleName = "Administrator";

		public AspNetRole() :
			this(string.Empty)
		{

		}

		public AspNetRole(string roleName)
		{
			this.Name = roleName;
		}
	}
}
