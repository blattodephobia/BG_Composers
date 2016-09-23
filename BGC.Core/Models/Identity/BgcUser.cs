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

        public virtual ICollection<Setting> UserSettings { get; set; }

		public BgcUser() :
			this(string.Empty)
		{
		}

		public BgcUser(string username)
		{
			this.UserName = username;
		}

        public IEnumerable<Permission> GetPermissions()
        {
            return Roles.SelectMany(userRole => userRole.Role.Permissions);
        }

        public T FindPermission<T>() where T : Permission
        {
            T result = GetPermissions().OfType<T>().FirstOrDefault();
            return result;
        }
	}
}
