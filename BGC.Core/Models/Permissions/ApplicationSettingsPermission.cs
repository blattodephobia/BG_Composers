using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public class ApplicationSettingsPermission : SettingsWritePermission
    {
        public override Type GetSettingsEntityTypeMapping()
        {
            return typeof(ApplicationSetting);
        }
    }
}
