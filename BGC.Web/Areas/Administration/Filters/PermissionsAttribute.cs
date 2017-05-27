using BGC.Core;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration
{
    /// <summary>
    /// Specifies the necessary permissions to perform an action. All of the required permissions have to be present for
    /// this attribute to mark an action as allowed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionsAttribute : Attribute
    {
        public IReadOnlyCollection<Type> PermissionTypes { get; private set; }

        public PermissionsAttribute(params Type[] permissionTypes)
        {
            PermissionTypes = new HashSet<Type>(permissionTypes ?? Enumerable.Empty<Type>());
        }

        public bool IsAuthorized(IEnumerable<IPermission> assignedPermissions)
        {
            Shield.IsNotNullOrEmpty(assignedPermissions).ThrowOnError();

            return assignedPermissions.Where(p => PermissionTypes.Contains(p.GetType())).Count() == PermissionTypes.Count;
        }
    }
}