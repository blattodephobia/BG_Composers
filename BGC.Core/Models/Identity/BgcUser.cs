using BGC.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
	public class BgcUser : IdentityUser<long, BgcUserLogin, BgcUserRole, BgcUserClaim>
	{
		public static readonly string AdministratorUserName = "Administrator";

		public bool MustChangePassword { get; set; }

		public BgcUser() :
			this(string.Empty)
		{
		}

		public BgcUser(string username)
		{
			this.UserName = username;
		}

        public T FindPermission<T>() where T : Permission
        {
            T result = this.Roles
                .SelectMany(ur => ur.Role.Permissions)
                .OfType<T>()
                .FirstOrDefault();

            return result;
        }
	}
}
