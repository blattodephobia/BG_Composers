using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
{
    [TestFixture]
    public class GetDependencySourcesTests
    {
        private class DependencyValueProxy : DependencyValue<int>
        {
            [DependencyPrecedence(0)]
            public SingleValueDependencySource<int> Source1 { get; private set; }

            [DependencyPrecedence(1)]
            public DependencySource<int> Source2 { get; private set; }

            public DependencySource<int> NoPrecedenceSource { get; private set; }

            public DependencySource<bool> WrongSourceType { get; private set; }

            public IEnumerable<DependencySource<int>> GetDependencySourcesProxy() => GetDependencySources();

            public DependencyValueProxy()
            {
                Source1 = new SingleValueDependencySource<int>();
                Source2 = new MultiValueFifoDependencySource<int>();
            }
        }

        [Test]
        public void ReturnsCorrectNumberOfDependencySources()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            Assert.AreEqual(3, proxy.GetDependencySourcesProxy().Count());
        }

        [Test]
        public void ReturnsDependencySourcesInCorrectOrder()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            List<DependencySource<int>> sources = proxy.GetDependencySourcesProxy().ToList();
            Assert.IsTrue(sources[0] is MultiValueFifoDependencySource<int>);
            Assert.IsTrue(sources[1] is SingleValueDependencySource<int>);
            Assert.IsTrue(sources[2] is DefaultValueDependencySource<int>);
        }
    }

    [TestFixture]
    public class EffectiveValueTests
    {
        private class DependencyValueTestHelper : DependencyValue<int>
        {
            [DependencyPrecedence(1)]
            public SingleValueDependencySource<int> Source1 { get; private set; }

            [DependencyPrecedence(0)]
            public SingleValueDependencySource<int> Source2 { get; private set; }

            public DependencyValueTestHelper() :
                this(0)
            {
            }

            public DependencyValueTestHelper(int defaultValue) :
                base(defaultValue)
            {
                Source1 = new SingleValueDependencySource<int>();
                Source2 = new SingleValueDependencySource<int>();
            }
        }

        [Test]
        public void GetsHighestPriorityValueFirst1()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper();
            val.Source1.SetValue(23);
            Assert.AreEqual(23, val.EffectiveValue);
        }

        [Test]
        public void GetsHighestPriorityValueFirst2()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper();
            val.Source2.SetValue(10);
            val.Source1.SetValue(23);
            Assert.AreEqual(23, val.EffectiveValue);
        }

        [Test]
        public void ReturnsDefaultValueWhenNoneIsSet()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper(17);
            Assert.AreEqual(17, val.EffectiveValue);
        }

        [Test]
        public void ReturnsDefaultValueWhenAllAreUnset()
        {
            DependencyValueTestHelper val = new DependencyValueTestHelper(17);
            val.Source1.SetValue(2);
            Assert.AreEqual(2, val.EffectiveValue);
            val.Source1.UnsetValue();
            Assert.AreEqual(17, val.EffectiveValue);
        }
    }
}
