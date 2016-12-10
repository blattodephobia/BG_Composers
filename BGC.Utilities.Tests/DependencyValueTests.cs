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
            public DependencySource<int> Source1 { get; private set; }

            public DependencySource<int> Source2 { get; private set; }

            public IEnumerable<DependencySource<int>> GetDependencySourcesProxy() => GetDependencySources();
        }

        [Test]
        public void EnumeratesInternalDependencySources()
        {
            DependencyValueProxy proxy = new DependencyValueProxy();
            Assert.AreEqual(3, proxy.GetDependencySourcesProxy().Count());
        }
    }
}
