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
            public DependencySource<int> Source1 { get; private set; }

            [DependencyPrecedence(1)]
            public DependencySource<int> Source2 { get; private set; }

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
}
