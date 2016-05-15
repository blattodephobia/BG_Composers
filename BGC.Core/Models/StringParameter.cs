using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class StringParameter : Parameter
    {
        public virtual string StringValue { get; set; }

        public sealed override string ToString()
        {
            return StringValue;
        }
    }
}
