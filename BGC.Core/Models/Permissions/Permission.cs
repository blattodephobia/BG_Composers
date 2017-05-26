using BGC.Utilities;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    [DiscoverableHierarchy(typeof(AdministratorRole), typeof(BgcRoleManager))]
    public abstract class Permission : BgcEntity<long>, IPermission
    {
        private string _name;

        /// <summary>
        /// Identifies the permissions in db stores.
        /// This property's value should be the same as the one returned by the GetType().FullName property.
        /// Although this property should technically be a read-only property, the setter is provided for compatibility
        /// with ORM frameworks.
        /// </summary>
        [Required]
        public string Name
        {
            get
            {
                return _name ?? (_name = GetType().FullName); ; 
            }

            protected set
            {
                Shield.Assert(
                    value,
                    value == GetType().FullName,
                    s => new InvalidOperationException($"Invalid relational mapping detected. Property {nameof(Name)} and the type name returned by the GetType().FullName must match. Expected {GetType().FullName}, actual value is {s}"));
                _name = value;
            }
        }

        private ICollection<BgcRole> _roles;
        public virtual ICollection<BgcRole> Roles
        {
            get
            {
                return _roles ?? (_roles = new HashSet<BgcRole>());
            }

            set
            {
                _roles = value;
            }
        }

        public bool Equals(IPermission other) => string.IsNullOrWhiteSpace(other?.Name) ? false : other.Name.CompareTo(Name) == 0;

        public sealed override bool Equals(object obj) => Equals(obj as Permission);

        public sealed override int GetHashCode() => Name.GetHashCode();
    }
}
