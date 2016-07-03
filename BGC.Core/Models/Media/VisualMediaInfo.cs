using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class VisualMediaInfo : MediaTypeInfo
    {
        public int Width { get; protected set; }

        public int Height { get; protected set; }
    }
}
