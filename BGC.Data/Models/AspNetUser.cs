using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Data.Models
{
	public class AspNetUser : IdentityUser<long, AspNetUserLogin, AspNetUserRole, AspNetUserClaim>
	{
		public static readonly string AdministratorUserName = "Administrator";

		public bool MustChangePassword { get; set; }

		public AspNetUser() :
			this(string.Empty)
		{

		}

		public AspNetUser(string username)
		{
			this.UserName = username;
		}
	}
}
