using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    public static class EnvironmentUtilities
    {
        public static readonly UriKind DotNetRelativeOrAbsolute = Type.GetType("Mono.Runtime") == null
            ? UriKind.RelativeOrAbsolute
            : (UriKind)300;
    }
}
