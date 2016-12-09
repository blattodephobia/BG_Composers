using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [Discoverable]
    public class FreelyDiscoverableType
    {
    }

    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void InitializesConsumingTypeWithReflection()
        {
            DiscoveredTypes obj = TypeDiscovery.Discover();
            Assert.AreEqual(typeof(CtorTests), obj.ConsumingType);
        }
    }

    [TestFixture]
    public class AssemblyPredicateTests
    {

        public static readonly List<Assembly> DynamicAssemblies = new List<Assembly>();

        [Test]
        public void FiltersAssemblies1()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            DiscoveredTypes t = TypeDiscovery.Discover(assemblyPredicate: a => a.GetName().FullName == executingAssembly.GetName().FullName);
            Assert.IsTrue(t.AllDiscoveredTypes().All(discoveredType => discoveredType.Assembly == executingAssembly));
        }

        [Test]
        public void FiltersAssemblies2()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            DiscoveredTypes t = TypeDiscovery.Discover(assemblyPredicate: a => a.GetName().FullName != executingAssembly.GetName().FullName);
            Assert.IsFalse(t.AllDiscoveredTypes().Any());
        }

        [Test]
        public void FiltersConsumingTypes1()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            DiscoveredTypes t = TypeDiscovery.Discover(assemblyPredicate: a => a.GetName().FullName != executingAssembly.GetName().FullName);
            Assert.IsFalse(t.AllDiscoveredTypes().Any());
        }
    }

    [TestFixture]
    public class ModeTests
    {
        [Discoverable(typeof(ModeTests))]
        public class RestrictedDiscoverableType
        {
        }

        public class ModeTestDerivedConsumer : ModeTests
        {

        }

        [Test]
        public void DiscoversRestrictedTypesOnlyInStrictMode()
        {
            DiscoveredTypes t = TypeDiscovery.Discover(mode: TypeDiscoveryMode.Strict);
            Assert.AreEqual(typeof(RestrictedDiscoverableType), t.AllDiscoveredTypes().Single());
        }

        [Test]
        public void DiscoversNonRestrictedTypes()
        {
            DiscoveredTypes t = TypeDiscovery.Discover(mode: TypeDiscoveryMode.Loose);
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(FreelyDiscoverableType)));
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(RestrictedDiscoverableType)));
            Assert.AreEqual(2, t.AllDiscoveredTypes().Count());
        }

        [Test]
        public void DiscoversWithDerivedConsumerTypes()
        {
            DiscoveredTypes t = TypeDiscovery.DiscoverFor<ModeTestDerivedConsumer>(mode: TypeDiscoveryMode.Loose);
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(FreelyDiscoverableType)));
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(RestrictedDiscoverableType)));
            Assert.AreEqual(2, t.AllDiscoveredTypes().Count());

        }
    }

    [TestFixture]
    public class DiscoverableHierarchyTests
    {
        [DiscoverableHierarchy(typeof(DiscoverableHierarchyTests))]
        public class BaseDiscoverable1
        {
        }

        public class DerivedDiscoverable1 : BaseDiscoverable1
        {
        }

        [Discoverable(typeof(DiscoverableHierarchyTests))]
        public class BaseDiscoverable2
        {
        }

        [DiscoverableHierarchy(typeof(DiscoverableHierarchyTests))]
        public class DerivedDiscoverable2 : BaseDiscoverable2
        {
        }

        [Test]
        public void DiscoversDerivedTypes1()
        {
            DiscoveredTypes t = TypeDiscovery.Discover();
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(DerivedDiscoverable1)));
        }

        [Test]
        public void DiscoversDerivedTypes2()
        {
            DiscoveredTypes t = TypeDiscovery.Discover();
            Assert.IsTrue(t.AllDiscoveredTypes().Contains(typeof(DerivedDiscoverable2)));
        }

        [Test]
        public void DiscoversDerivedTypes3()
        {
            DiscoveredTypes t = TypeDiscovery.Discover(mode: TypeDiscoveryMode.Loose);
            List<Type> inheritingTypes = t.DiscoveredTypesInheritingFrom<BaseDiscoverable1>().ToList();
            Assert.AreEqual(2, inheritingTypes.Count);
            Assert.IsTrue(inheritingTypes.Contains(typeof(BaseDiscoverable1)));
            Assert.IsTrue(inheritingTypes.Contains(typeof(DerivedDiscoverable1)));
        }
    }

    [TestFixture]
    public class FromLambdaFunctionDiscoveryTests
    {
        [Test]
        public void DoesntDiscoverAnonymousTypesInAnonymousMethods()
        {
            Lazy<DiscoveredTypes> td = new Lazy<DiscoveredTypes>(() => TypeDiscovery.Discover());
            Assert.AreEqual(typeof(Lazy<>), td.Value.ConsumingType);
        }
    }

    [TestFixture]
    public class CacheTests
    {
        [Test]
        public void CreatesNewDiscoveryAfterGarbageCollection()
        {
            // this will call the CLR implementation of GetHashCode();
            // different references will produce different hash codes, even if the object is logically the same
            int oldHashCode = TypeDiscovery.FindDiscoveryFor<CacheTests>().GetHashCode();
            GC.Collect();
            int newHashCode = TypeDiscovery.FindDiscoveryFor<CacheTests>().GetHashCode();

            Assert.AreNotEqual(oldHashCode, newHashCode);
        }

        [Test]
        public void ReturnsSameDiscoveryBetweenCalls1()
        {
            DiscoveredTypes discovery1 = TypeDiscovery.FindDiscoveryFor<CacheTests>();
            DiscoveredTypes discovery2 = TypeDiscovery.FindDiscoveryFor<CacheTests>();

            Assert.AreSame(discovery1, discovery2);
        }

        [Test]
        public void ReturnsSameDiscoveryBetweenCalls2()
        {
            int oldHashCode = TypeDiscovery.FindDiscoveryFor<CacheTests>().GetHashCode();
            int newHashCode = TypeDiscovery.FindDiscoveryFor<CacheTests>().GetHashCode();

            Assert.AreEqual(oldHashCode, newHashCode);
        }
    }
}
