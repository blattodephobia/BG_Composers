using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Models
{
    public abstract class MediaTypeInfo
    {
        public abstract bool ValidateHeader(Stream stream);
    }
}
