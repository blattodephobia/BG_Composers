using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public sealed class UserSettingsPermission : SettingsWritePermission, IUserSettingsPermission
    {
        internal sealed override SettingPriority GetSettingsPriority()
        {
            return SettingPriority.User;
        }

        public override string Name => nameof(IUserSettingsPermission);
    }
}
