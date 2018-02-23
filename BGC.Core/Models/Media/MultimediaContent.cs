using CodeShield;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class MultimediaContent
    {
        public MediaTypeInfo Metadata { get; set; }
        
        public Stream Data { get; set; }

        public MultimediaContent(Stream data = null, MediaTypeInfo metadata = null)
        {
            Data = data;
            Metadata = metadata;
        }
    }
}
