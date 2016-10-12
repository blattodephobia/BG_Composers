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
            TypeDiscoveryProvider obj = new TypeDiscoveryProvider();
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
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(assemblyPredicate: a => a.GetName().FullName == executingAssembly.GetName().FullName);
            Assert.IsTrue(t.DiscoveredTypes.All(discoveredType => discoveredType.Assembly == executingAssembly));
        }

        [Test]
        public void FiltersAssemblies2()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(assemblyPredicate: a => a.GetName().FullName != executingAssembly.GetName().FullName);
            Assert.IsFalse(t.DiscoveredTypes.Any());
        }

        [Test]
        public void FiltersConsumingTypes1()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(assemblyPredicate: a => a.GetName().FullName != executingAssembly.GetName().FullName);
            Assert.IsFalse(t.DiscoveredTypes.Any());
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
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(mode: TypeDiscoveryMode.Strict);
            Assert.AreEqual(typeof(RestrictedDiscoverableType), t.DiscoveredTypes.Single());
        }

        [Test]
        public void DiscoversNonRestrictedTypes()
        {
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(mode: TypeDiscoveryMode.Loose);
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(FreelyDiscoverableType)));
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(RestrictedDiscoverableType)));
            Assert.AreEqual(2, t.DiscoveredTypes.Count());
        }

        [Test]
        public void DiscoversWithDerivedConsumerTypes()
        {
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(consumingType: typeof(ModeTestDerivedConsumer), mode: TypeDiscoveryMode.Loose);
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(FreelyDiscoverableType)));
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(RestrictedDiscoverableType)));
            Assert.AreEqual(2, t.DiscoveredTypes.Count());

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
            TypeDiscoveryProvider t = new TypeDiscoveryProvider();
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(DerivedDiscoverable1)));
        }

        [Test]
        public void DiscoversDerivedTypes2()
        {
            TypeDiscoveryProvider t = new TypeDiscoveryProvider();
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(DerivedDiscoverable2)));
        }
    }
}
