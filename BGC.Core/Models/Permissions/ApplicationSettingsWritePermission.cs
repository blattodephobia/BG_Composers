using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ApplicationSettingsWritePermission : SettingsWritePermission
    {
        public sealed override SettingPriority GetSettingsPriority()
        {
            return SettingPriority.Application;
        }
    }
}
