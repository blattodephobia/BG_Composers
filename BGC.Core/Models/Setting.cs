using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class Setting : BgcEntity<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Parameter Value { get; set; }

        public abstract int Priority { get; }
    }
}
