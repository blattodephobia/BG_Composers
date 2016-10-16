using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    [DebuggerDisplay("{Value}", Name = "{Name}", Type = "{Value?.GetType()}")]
    public sealed class DebuggerDisplayKeyValuePair
    {
        public string Name { get; private set; }

        public object Value { get; private set; }

        public DebuggerDisplayKeyValuePair(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
