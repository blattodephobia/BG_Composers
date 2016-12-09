using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MappableWithAttribute : Attribute
    {
        public Type RelatedType { get; private set; }

        public MappableWithAttribute(Type relatedType)
        {
            RelatedType = relatedType;
        }
    }
}
