using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class Parameter : BgcEntity<long>
    {
        public virtual string StringValue { get; set; }

        public sealed override string ToString()
        {
            return StringValue;
        }
    }
}
