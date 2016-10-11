using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    [Flags]
    public enum TypeDiscoveryMode
    {
        /// <summary>
        /// Discovers only those types that are explicitly set to be discoverable by the type that attempts to discover them.
        /// </summary>
        Strict = 0,

        /// <summary>
        /// Discover types that are discoverable by a specific type and those that are discoverable by any type.
        /// </summary>
        Loose = 1
    }
}
