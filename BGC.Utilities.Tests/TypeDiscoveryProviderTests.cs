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

    [Discoverable]
    public class FreelyDiscoverableType
    {
    }

    [Discoverable(typeof(ModeTests))]
    public class RestrictedDiscoverableType
    {
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
        public class ModeTestDerived : ModeTests
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
        public void DiscoversDerivedTypes()
        {
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(consumingType: typeof(ModeTestDerived), mode: TypeDiscoveryMode.Loose);
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(FreelyDiscoverableType)));
            Assert.IsTrue(t.DiscoveredTypes.Contains(typeof(RestrictedDiscoverableType)));
            Assert.AreEqual(2, t.DiscoveredTypes.Count());

        }
    }
}
