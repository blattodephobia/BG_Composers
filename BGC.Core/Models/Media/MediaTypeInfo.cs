using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class MediaTypeInfo
    {
        public abstract bool ValidateHeader();

        public abstract ContentType MimeType { get; }
    }
}
