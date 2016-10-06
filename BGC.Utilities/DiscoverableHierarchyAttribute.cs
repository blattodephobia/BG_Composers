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
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public sealed class DiscoverableHierarchyAttribute : Attribute
    {
        /// The types that will instantiate the type this attribute is applied on.
        public Type[] ConsumingTypes { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="DiscoverableHierarchyAttribute"/> 
        /// </summary>
        /// <param name="consumingTypes">The types that will instantiate the type this attribute is applied on. Use null to make the type discoverable by any consuming type.</param>
        public DiscoverableHierarchyAttribute(Type[] consumingTypes = null)
        {
            ConsumingTypes = consumingTypes;

        }
    }
}
