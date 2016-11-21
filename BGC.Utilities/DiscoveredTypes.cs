using CodeShield;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Used to discover and cache a list of all types in all non-dynamic loaded assemblies on which the <see cref="DiscoverableAttribute"/> attribute has been applied.
    /// </summary>
    public class DiscoveredTypes
    {
        private readonly IEnumerable<Assembly> assembliesToSearch;
        private readonly TypeDiscoveryMode mode;
        
        private Lazy<Type[]> _enumeratedTypes;

        /// <summary>
        /// Returns all types discovered by this instance of <see cref="DiscoveredTypes"/>.
        /// </summary>
        /// <returns></returns>
        public Type[] AllDiscoveredTypes() => _enumeratedTypes.Value;

        /// <summary>
        /// Returns all types discovered by this instance of <see cref="DiscoveredTypes"/> that directly or indirectly inherit from or implement <typeparamref name="TBase"/>.
        /// The <typeparamref name="TBase"/> type, if discoverable by the current <see cref="DiscoveredTypes"/> will also be included in the results.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <returns></returns>
        public IEnumerable<Type> DiscoveredTypesInheritingFrom<TBase>() => AllDiscoveredTypes().Where(discoveredType => typeof(TBase).IsAssignableFrom(discoveredType));

        public Type ConsumingType { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DiscoveredTypes"/> with the specified parameters.
        /// </summary>
        
        internal DiscoveredTypes(Type consumingType = null, Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            ConsumingType = consumingType;
            assemblyPredicate = assemblyPredicate ?? delegate (Assembly a) { return true; };
            this.assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && assemblyPredicate.Invoke(a));
            this.mode = mode;
        }

        internal DiscoveredTypes(IEnumerable<Type> discoveredTypes, Type consumingType)
        {
            _enumeratedTypes = new Lazy<Type[]>(() => discoveredTypes.ToArray());
            ConsumingType = consumingType;
        }
    }
}
