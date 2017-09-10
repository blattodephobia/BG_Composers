using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Indicates that the type this attribute is applied on can be dynamically discovered and instantiated at runtime by
    /// a different consumer type. This attribute is inherited and types deriving from classes decorated with this attribute
    /// will be discovered as well. To discover only classes decorated with a <see cref="TypeDiscoveryAttribute"/> and none
    /// of their children, use <see cref="DiscoverableAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class DiscoverableHierarchyAttribute : TypeDiscoveryAttribute
    {
        /// <summary>
        /// Creates a new instance of the <see cref="DiscoverableHierarchyAttribute"/> 
        /// </summary>
        /// <param name="consumingTypes">The types that will instantiate the type this attribute is applied on. Use null to make the type discoverable by any consuming type.</param>
        public DiscoverableHierarchyAttribute(params Type[] consumingTypes) :
            base(consumingTypes)
        {
        }
    }
}
