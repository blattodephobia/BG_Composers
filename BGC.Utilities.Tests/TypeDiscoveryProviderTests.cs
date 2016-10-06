using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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

    [TestFixture]
    public class AssemblyPredicateTests
    {
        static readonly Assembly[] DynamicAssemblies = new Assembly[2];
        [Test]
        public void FiltersAssemblies()
        {
            AssemblyBuilder dynamicAssembly1 = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Dynamic1"), AssemblyBuilderAccess.ReflectionOnly);
            TypeBuilder.
            TypeDiscoveryProvider t = new TypeDiscoveryProvider(assemblyPredicate: a => true);
        }
    }
}
