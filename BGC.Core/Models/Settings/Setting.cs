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

        public virtual string StringValue { get; set; }

        public SettingPriority Priority { get; set; }

        public sealed override string ToString()
        {
            return $"{Name}: {StringValue}";
        }
    }
}
