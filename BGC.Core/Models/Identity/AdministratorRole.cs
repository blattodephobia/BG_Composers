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
    public sealed class AdministratorRole : BgcRole
    {
        public override bool CanDelete => false;

        public AdministratorRole()
        {
            Name = nameof(AdministratorRole);
            Permissions.Add(new SendInvitePermission());
            Permissions.Add(new ApplicationSettingsWritePermission());
            Permissions.Add(new UserSettingsPermission());
        }
    }
}
