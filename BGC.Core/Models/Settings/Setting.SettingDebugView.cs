using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class Setting
    {
        internal sealed class SettingDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public DebuggerDisplayKeyValuePair[] Properties { get; private set; }

            public SettingDebugView(Setting setting)
            {
                Properties = setting.GetType().GetProperties().Select(property => new DebuggerDisplayKeyValuePair(property.Name, property.GetValue(setting))).ToArray();
            }
        }
    }
}
