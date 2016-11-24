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
    public static class TypeDiscovery
    {
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

        private static IEnumerable<Type> GetTypeDiscoveryQuery(IEnumerable<Assembly> assembliesToSearch, Type consumingType, TypeDiscoveryMode mode)
        {
            var query = from type in (from assembly in assembliesToSearch
                                      from exportedTypes in TryGetExportedTypes(assembly)
                                      where !assembly.IsDynamic
                                      select exportedTypes)
                        let discoverableAttributes = type.GetCustomAttributes<TypeDiscoveryAttribute>()
                        let matchesConsumingType = (from typeDiscoveryAttribute in discoverableAttributes ?? Enumerable.Empty<TypeDiscoveryAttribute>()
                                                    from declaredConsumingType in typeDiscoveryAttribute.ConsumingTypes
                                                    where declaredConsumingType.IsAssignableFrom(consumingType)
                                                    select declaredConsumingType).Any()
                        let isNonRestrictedType = discoverableAttributes?.Any(t => t.IsFreelyDiscoverable) ?? false
                        where matchesConsumingType || (mode.HasFlag(TypeDiscoveryMode.Loose) && isNonRestrictedType)
                        select type;
            return query;
        }

        private static readonly Dictionary<TypeDiscoveryCacheKey, WeakReference> CachedDiscoveries = new Dictionary<TypeDiscoveryCacheKey, WeakReference>();

        /// <summary>
        /// Attempts to get a <see cref="DiscoveredTypes"/> from a weakly-referenced cache. If no such object exists, creates a new instance
        /// and stores it in that cache.
        /// </summary>
        /// <typeparam name="TConsumer">The type for which discoverable types will be found.</typeparam>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="mode">When set to <see cref="TypeDiscoveryMode.Strict"/>, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        /// <returns></returns>
        public static DiscoveredTypes FindDiscoveryFor<TConsumer>(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            lock (CachedDiscoveries)
            {
                TypeDiscoveryCacheKey key = new TypeDiscoveryCacheKey(typeof(TConsumer), assemblyPredicate, mode);
                if (!CachedDiscoveries.ContainsKey(key))
                {
                    CachedDiscoveries.Add(key, new WeakReference(DiscoverFor<TConsumer>(assemblyPredicate, mode)));
                }

                WeakReference @ref = CachedDiscoveries[key];
                return (@ref.Target ?? (@ref.Target = DiscoverFor<TConsumer>(assemblyPredicate, mode))) as DiscoveredTypes;
            }
        }

        /// <summary>
        /// Discovers types for a given consumer using the assemblies loaded in the current <see cref="AppDomain"/>.
        /// </summary>
        /// <typeparam name="TConsumer">The type for which discoverable types will be found.</typeparam>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="mode">When set to <see cref="TypeDiscoveryMode.Strict"/>, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        /// <returns></returns>
        public static DiscoveredTypes DiscoverFor<TConsumer>(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            return Discover(typeof(TConsumer), assemblyPredicate, mode);
        }

        /// <summary>
        /// Discovers types for the type that calls this method using the assemblies loaded in the current <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="mode">When set to <see cref="TypeDiscoveryMode.Strict"/>, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        /// <returns></returns>
        public static DiscoveredTypes Discover(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            Type callerType = new StackTrace(1).GetFrames().Select(f => f.GetMethod().DeclaringType).FirstOrDefault(type => !IsAnonymousType(type));
            return Discover(callerType, assemblyPredicate, mode);
        }

        /// <summary>
        /// Discovers types for a given consumer using the assemblies loaded in the current <see cref="AppDomain"/>.
        /// </summary>
        /// <param name="consumingType">The type for which discoverable types will be found.</param>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="mode">When set to <see cref="TypeDiscoveryMode.Strict"/>, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        /// <returns></returns>
        public static DiscoveredTypes Discover(Type consumingType, Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            return Discover(consumingType, AppDomain.CurrentDomain.GetAssemblies(), assemblyPredicate, mode);
        }

        /// <summary>
        /// Discovers types for a given consumer from the provided assemblies.
        /// </summary>
        /// <param name="consumingType">The type for which discoverable types will be found. Use null to set the consuming type to the type calling the constructor via reflection.</param>
        /// <param name="assemblies">The assemblies that will be searched.</param>
        /// <param name="assemblyPredicate">Filters the loaded assemblies in the current AppDomain. Only those that match the predicate will be searched.</param>
        /// <param name="mode">When set to <see cref="TypeDiscoveryMode.Strict"/>, only types that are discoverable by a specific consuming type will be returned.
        /// Otherwise, types whose <see cref="DiscoverableAttribute"/> doesn't specify a specific consuming type will also be included.</param>
        public static DiscoveredTypes Discover(Type consumingType, IEnumerable<Assembly> assemblies, Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            Shield.ArgumentNotNull(consumingType).ThrowOnError();
            Shield.ArgumentNotNull(assemblies).ThrowOnError();

            var filteredAssemblies = assemblyPredicate != null
                ? assemblies.Where(a => !a.IsDynamic && assemblyPredicate.Invoke(a))
                : assemblies;

            return new DiscoveredTypes(GetTypeDiscoveryQuery(filteredAssemblies, consumingType, mode), consumingType);
        }
    }
}
