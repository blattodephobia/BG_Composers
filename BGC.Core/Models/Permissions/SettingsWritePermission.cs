using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class SettingsWritePermission : Permission
    {
        internal abstract SettingPriority GetSettingsPriority();

        internal SettingsWritePermission()
        {
        }
    }
}
