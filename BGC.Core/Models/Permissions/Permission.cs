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
        public abstract string Name { get; }

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
