using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public abstract class TypeDiscoveryAttribute : Attribute
    {
        /// <summary>
        /// The types that will instantiate the type this attribute is applied on.
        /// </summary>
        public IReadOnlyList<Type> ConsumingTypes { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether there are any declared consumer types in the <see cref="ConsumingTypes"/>.
        /// </summary>
        public bool IsFreelyDiscoverable => !(ConsumingTypes?.Any() ?? false);

        protected TypeDiscoveryAttribute(Type[] consumingTypes = null)
        {
            ConsumingTypes = consumingTypes;
        }
    }
}
