using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class Setting : BgcEntity<long>, IParameter<string>
    {
        public string Name { get; set; }

        [Unicode]
        public string Description { get; set; }

        [Unicode]
        public virtual string StringValue { get; set; }

        public SettingPriority Priority { get; set; }

        public sealed override string ToString()
        {
            return $"{Name}: {StringValue}";
        }

        string IParameter<string>.Value
        {
            get
            {
                return StringValue;
            }

            set
            {
                StringValue = value;
            }
        }
    }
}
