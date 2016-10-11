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
        private readonly IEnumerable<Assembly> assembliesToSearch;
        private readonly TypeDiscoveryMode mode;

        private IEnumerable<Type> discoveredTypes;
        public IEnumerable<Type> DiscoveredTypes
        {
            get
            {
                return this.discoveredTypes ?? (this.discoveredTypes =
                    from type in this.assembliesToSearch.SelectMany(a => a.GetExportedTypes()).ToList()
                    let discoverableAttribute = type.GetCustomAttribute<DiscoverableAttribute>()
                    let matchesConsumingType = discoverableAttribute?.ConsumingTypes?.Any(baseType => baseType.IsAssignableFrom(ConsumingType)) ?? false
                    let isNonRestrictedType = discoverableAttribute != null && !(discoverableAttribute.ConsumingTypes?.Any() ?? false) // is not restricted when ConsumingTypes is null or it's empty
                    where matchesConsumingType || (isNonRestrictedType && this.mode.HasFlag(TypeDiscoveryMode.Loose))
                    select type);
            }
        }

        public Type ConsumingType { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="TypeDiscoveryProvider"/> with the specified parameters.
        /// </summary>
        /// <param name="consumingType">The type for which discoverable types will be found. Use null to set the consuming type to the type calling the constructor via reflection.</param>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="useStrictMode">When set to true, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        public TypeDiscoveryProvider(Type consumingType = null, Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            ConsumingType = consumingType ?? new StackTrace(1).GetFrame(0).GetMethod().DeclaringType;
            assemblyPredicate = assemblyPredicate ?? delegate (Assembly a) { return true; };
            this.assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && assemblyPredicate.Invoke(a));
            this.mode = mode;
        }

        public IEnumerable<Type> GetDiscoveredInheritanceChain<TBase>() => DiscoveredTypes.Where(discoveredType => typeof(TBase).IsAssignableFrom(discoveredType));
    }
}
