using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Indicates that the type this attribute is applied on will be used at runtime by a different consumer type.
    /// This attribute is not inheritable. To make inheriting types discoverable with the same base-class-decorating
    /// attribute, use <see cref="DiscoverableHierarchyAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class DiscoverableAttribute : TypeDiscoveryAttribute
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DiscoverableAttribute"/> 
        /// </summary>
        /// <param name="consumingTypes">The types that will instantiate the type this attribute is applied on. Use null to make the type discoverable by any consuming type.</param>
        public DiscoverableAttribute(params Type[] consumingTypes) :
            base(consumingTypes)
        {
        }
    }
}
