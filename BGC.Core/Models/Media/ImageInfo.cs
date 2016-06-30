using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class ImageInfo : MediaTypeInfo
    {
        public int Width { get; protected set; }

        public int Height { get; protected set; }

        protected ImageInfo(Stream content) :
            base(content)
        {
        }
    }
}
