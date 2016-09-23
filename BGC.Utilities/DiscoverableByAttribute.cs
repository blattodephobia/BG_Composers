using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Indicates that the type this attribute is applied on will be used at runtime by a different consumer type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
    public sealed class DiscoverableByAttribute : Attribute
    {
        /// The type of the objects that will instantiate the type this attribute is applied on.
        public Type ConsumingType { get; private set; }

        /// <summary>
        /// Creates a new instance of the <see cref="DiscoverableByAttribute"/> 
        /// </summary>
        /// <param name="consumingType">The type of the objects that will instantiate the type this attribute is applied on.</param>
        public DiscoverableByAttribute(Type consumingType)
        {
            ConsumingType = consumingType;
        }
    }
}
