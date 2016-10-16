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
    public abstract class Permission : BgcEntity<long>, IEquatable<Permission>
    {
        private string name;

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
                return this.name ?? (this.name = this.GetType().FullName); ; 
            }

            protected set
            {
                Shield.Assert(
                    value,
                    value == this.GetType().FullName,
                    s => new InvalidOperationException($"Invalid relational mapping detected. Property {nameof(Name)} and the type name returned by the GetType().FullName must match. Expected {this.GetType().FullName}, actual value is {s}"));
                this.name = value;
            }
        }

        private ICollection<BgcRole> roles;
        public virtual ICollection<BgcRole> Roles
        {
            get
            {
                return this.roles ?? (this.roles = new HashSet<BgcRole>());
            }

            set
            {
                this.roles = value;
            }
        }

        public bool Equals(Permission other) => (other?.Name ?? "").CompareTo(Name) == 0;

        public sealed override bool Equals(object obj) => Equals(obj as Permission);

        public sealed override int GetHashCode() => Name.GetHashCode();
    }
}
