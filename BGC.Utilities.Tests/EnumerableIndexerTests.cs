using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.EnumerableIndexerTests
{
    public class EnumerableIndexerTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEnumerateCallback()
        {
            Assert.Throws<ArgumentNullException>(() => new EnumerableIndexer<int, int>(x => x, (x, y) => { }, null));
        }

        [Test]
        public void CallsEnumerateCallbackWhenEnumerating()
        {
            int callbacks = 0;
            Func<IEnumerable<KeyValuePair<int, int>>> enumerateCallback = () =>
            {
                callbacks++;
                return Enumerable.Empty<KeyValuePair<int, int>>();
            };

            var indexer = new EnumerableIndexer<int, int>(x => x, (x, y) => { }, enumerateCallback);

            List<int> result = indexer.All().Select(kvp => kvp.Value).ToList();

            Assert.AreEqual(1, callbacks);
            Assert.AreEqual(0, result.Count);
        }
    }
}
