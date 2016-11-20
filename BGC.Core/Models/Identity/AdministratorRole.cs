using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    /// <summary>
    /// The role for users that have unrestricted access to the application.
    /// This role populates its default permissions when it's instantiated.
    /// </summary>
    public sealed class AdministratorRole : ApplicationCriticalRole
    {
        private static readonly IEnumerable<Permission> ApplicationPermissions =
            TypeDiscovery.Discover()
                .DiscoveredTypesInheritingFrom<Permission>()
                .Where(permission => !permission.IsAbstract)
                .Select(permission => (Permission)Activator.CreateInstance(permission))
                .ToList();

        public AdministratorRole()
        {
            Name = nameof(AdministratorRole);
            Permissions = new HashSet<Permission>(ApplicationPermissions);
        }
    }
}
