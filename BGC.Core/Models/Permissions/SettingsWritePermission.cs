using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public abstract class SettingsWritePermission : Permission
    {
        public abstract Type GetSettingsEntityTypeMapping();
    }
}
