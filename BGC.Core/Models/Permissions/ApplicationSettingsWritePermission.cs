using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public sealed class ApplicationSettingsWritePermission : SettingsWritePermission
    {
        internal sealed override SettingPriority GetSettingsPriority()
        {
            return SettingPriority.Application;
        }
    }
}
