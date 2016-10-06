using CodeShield;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Used to discover and cache a list of all types on which the <see cref="DiscoverableAttribute"/> attribute has been applied.
    /// </summary>
    public class TypeDiscoveryProvider
    {
        private IEnumerable<Assembly> assembliesToSearch;

        private IEnumerable<Type> discoveredTypes;
        public IEnumerable<Type> DiscoveredTypes
        {
            get
            {
                return this.discoveredTypes ?? (this.discoveredTypes = this.assembliesToSearch
                    .SelectMany(a => a.ExportedTypes)
                    .Where(t => t.GetCustomAttribute<DiscoverableAttribute>()?.ConsumingTypes.Contains(ConsumingType) ?? false));
            }
        }

        public Type ConsumingType { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of <see cref="TypeDiscoveryProvider"/> with the specified parameters.
        /// </summary>
        /// <param name="consumingType">The type for which discoverable types will be found. Use null to set the consuming type to the type calling the constructor via reflection.</param>
        /// <param name="assemblyPredicate"></param>
        public TypeDiscoveryProvider(Type consumingType = null, Func<Assembly, bool> assemblyPredicate = null)
        {
            ConsumingType = consumingType ?? new StackTrace(1).GetFrame(0).GetMethod().DeclaringType;
            assemblyPredicate = assemblyPredicate ?? delegate (Assembly a) { return true; };
            this.assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies().Where(assemblyPredicate ?? assemblyPredicate);
        }

        public IEnumerable<Type> GetDiscoveredInheritanceChain<TBase>()
        {
            return DiscoveredTypes.Where(discoveredType => typeof(TBase).IsAssignableFrom(discoveredType));
        }
    }
}
