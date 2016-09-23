using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities
{
    /// <summary>
    /// Used to discover and cache a list of all types on which the <see cref="DiscoverableByAttribute"/> attribute has been applied.
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
                    .Where(t => t.GetCustomAttribute<DiscoverableByAttribute>()?.ConsumingType == ConsumingType));
            }
        }

        public Type ConsumingType { get; private set; }

        public TypeDiscoveryProvider(Type consumingType, Func<Assembly, bool> assemblyPredicate = null)
        {
            ConsumingType = Shield.ArgumentNotNull(consumingType, nameof(consumingType));
            assemblyPredicate = assemblyPredicate ?? delegate (Assembly a) { return true; };
            this.assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies().Where(assemblyPredicate ?? assemblyPredicate);
        }

        public IEnumerable<Type> GetDiscoveredInheritanceChain<TBase>()
        {
            return DiscoveredTypes.Where(discoveredType => typeof(TBase).IsAssignableFrom(discoveredType));
        }
    }
}
