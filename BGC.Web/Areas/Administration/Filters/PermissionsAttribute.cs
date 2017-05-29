using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration
{
    /// <summary>
    /// Specifies the necessary permissions to perform an action. Any of the required permissions have to be present for
    /// this attribute to mark an action as allowed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class PermissionsAttribute : Attribute
    {
        public IReadOnlyCollection<string> PermissionTypes { get; private set; }

        public PermissionsAttribute(params string[] permissionTypes)
        {
            PermissionTypes = new HashSet<string>(permissionTypes ?? Enumerable.Empty<string>());
        }

        public bool IsAuthorized(IEnumerable<IPermission> assignedPermissions) => IsAuthorized(assignedPermissions?.Select(p => p?.Name));

        public bool IsAuthorized(IEnumerable<string> assignedPermissions)
        {
            Shield.IsNotNullOrEmpty(assignedPermissions).ThrowOnError();

            bool isAuthorized = !PermissionTypes.Any(); // isAuthorized is true if there are no required permissions
            if (!isAuthorized)
            {
                foreach (string permission in assignedPermissions)
                {
                    isAuthorized |= PermissionTypes.Contains(Shield.AssertOperation(permission, permission != null, $"Sequence contains null objects.").GetValueOrThrow());
                }
            }

            return isAuthorized;
        }
    }
}