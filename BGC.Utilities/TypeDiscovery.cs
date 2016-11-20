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

        private static readonly Dictionary<object, WeakReference> CachedDiscoveries = new Dictionary<object, WeakReference>();
        /// <summary>
        /// Attempts to get a <see cref="DiscoveredTypes"/> from a weakly-referenced cache. If no such object exists, creates a new instance
        /// and stores it in that cache.
        /// </summary>
        /// <typeparam name="TConsumer"></typeparam>
        /// <param name="assemblyPredicate"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static DiscoveredTypes FindDiscoveryFor<TConsumer>(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            lock (CachedDiscoveries)
            {
                WeakReference @ref = CachedDiscoveries.ContainsKey(null)
                    ? CachedDiscoveries[null]
                    : new WeakReference(DiscoverFor<TConsumer>(assemblyPredicate, mode));

                return (@ref.Target ?? (@ref.Target = DiscoverFor<TConsumer>(assemblyPredicate, mode))) as DiscoveredTypes;
            }
        }

        public static DiscoveredTypes DiscoverFor<TConsumer>(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            return new DiscoveredTypes(typeof(TConsumer), assemblyPredicate, mode);
        }

        public static DiscoveredTypes Discover(Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            Type callerType = new StackTrace(1).GetFrames().Select(f => f.GetMethod().DeclaringType).FirstOrDefault(type => !IsAnonymousType(type));
            return Discover(callerType, assemblyPredicate, mode);
        }

        public static DiscoveredTypes Discover(Type consumingType, Func<Assembly, bool> assemblyPredicate = null, TypeDiscoveryMode mode = TypeDiscoveryMode.Strict)
        {
            return new DiscoveredTypes(consumingType, assemblyPredicate, mode);
        }
    }
}
