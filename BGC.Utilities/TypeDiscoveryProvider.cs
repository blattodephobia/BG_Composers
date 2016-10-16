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
    public class TypeDiscoveryProvider
    {
        private readonly IEnumerable<Assembly> assembliesToSearch;
        private readonly TypeDiscoveryMode mode;

        private static bool IsAnonymousType(Type t)
        {
            return t.IsDefined(typeof(CompilerGeneratedAttribute))
                   && (t.Name.StartsWith("<>") || t.Name.StartsWith("VB$"))
                   && t.Attributes.HasFlag(TypeAttributes.NotPublic);
        }

        // This method is a wrapper around Assembly.GetExportedTypes(). Occasionally, that method will attempt to load an assembly that hasn't been loaded in the current application
        // domain and will throw an exception. The cause, as of the time of writing, is unknown.
        private static IEnumerable<Type> TryGetExportedTypes(Assembly a)
        {
            try
            {
                return a.GetExportedTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }

        private IEnumerable<Type> discoveredTypes;

        /// <summary>
        /// Returns all types discovered by this instance of <see cref="TypeDiscoveryProvider"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> AllDiscoveredTypes()
        {
            return this.discoveredTypes ?? (this.discoveredTypes =
                from type in this.assembliesToSearch.SelectMany(a => TryGetExportedTypes(a)).ToList()
                let discoverableAttributes = type.GetCustomAttributes<TypeDiscoveryAttribute>()
                let matchesConsumingType = (from typeDiscoveryAttribute in discoverableAttributes ?? Enumerable.Empty<TypeDiscoveryAttribute>()
                                            from consumingType in typeDiscoveryAttribute?.ConsumingTypes
                                            where consumingType.IsAssignableFrom(ConsumingType)
                                            select consumingType).Any()
                let isNonRestrictedType = discoverableAttributes?.Any(t => t.IsFreelyDiscoverable) ?? false
                where matchesConsumingType || (isNonRestrictedType && this.mode.HasFlag(TypeDiscoveryMode.Loose))
                select type);
        }

        /// <summary>
        /// Returns all types discovered by this instance of <see cref="TypeDiscoveryProvider"/> that directly or indirectly inherit from or implement <typeparamref name="TBase"/>.
        /// The <typeparamref name="TBase"/> type, if discoverable by the current <see cref="TypeDiscoveryProvider"/> will also be included in the results.
        /// </summary>
        /// <typeparam name="TBase"></typeparam>
        /// <returns></returns>
        public IEnumerable<Type> DiscoveredTypesInheritingFrom<TBase>() => AllDiscoveredTypes().Where(discoveredType => typeof(TBase).IsAssignableFrom(discoveredType));

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
            ConsumingType = consumingType
                            ?? new StackTrace(1).GetFrames().Select(f => f.GetMethod().DeclaringType).FirstOrDefault(type => !IsAnonymousType(type));
            assemblyPredicate = assemblyPredicate ?? delegate (Assembly a) { return true; };
            this.assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && assemblyPredicate.Invoke(a));
            this.mode = mode;
        }
    }
}
