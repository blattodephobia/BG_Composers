using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionsAttribute : Attribute
    {
        public IReadOnlyCollection<Type> PermissionTypes { get; private set; }

        public PermissionsAttribute(params Type[] permissionTypes)
        {
            PermissionTypes = new HashSet<Type>(permissionTypes ?? Enumerable.Empty<Type>());
        }
    }
}