using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.EnumerableExtensionsTests
{
    [TestFixture]
    public class ToStringAggregateTests
    {
        [Test]
        public void AggregatesWithNoDelimiter()
        {
            string[] enumerable = new[] { "1", "2", "3" };
            string aggregate = enumerable.ToStringAggregate();
            Assert.AreEqual("123", aggregate);
        }

        [Test]
        public void AggregatesWithDelimiter()
        {
            string[] enumerable = new[] { "1", "2", "3" };
            string aggregate = enumerable.ToStringAggregate("#");
            Assert.AreEqual("1#2#3", aggregate);
        }

        [Test]
        public void AggregatesWithSingleElementNoDelimiter()
        {
            string[] enumerable = new[] { "1" };
            string aggregate = enumerable.ToStringAggregate();
            Assert.AreEqual("1", aggregate);
        }

        [Test]
        public void AggregatesWithSingleElementWithDelimiter()
        {
            string[] enumerable = new[] { "1" };
            string aggregate = enumerable.ToStringAggregate("asd");
            Assert.AreEqual("1", aggregate);
        }

        [Test]
        public void ThrowsOnNullCollection()
        {
            IEnumerable<string> nullCollection = null;
            Assert.Throws<ArgumentNullException>(() => nullCollection.ToStringAggregate("asd"));
        }
    }

    public class AddRangeTests : TestFixtureBase
    {
        [Test]
        public void AddsItemsIfCollection()
        {
            var coll = new Collection<int>();
            var items = new[] { 1, 2, 3 };
            EnumerableExtensions.AddRange(coll, items);

            Assert.IsTrue(items.SequenceEqual(coll));
        }

        [Test]
        public void AddsItemsIfList()
        {
            ICollection<int> list = new List<int>();
            var items = new[] { 1, 2, 3 };
            EnumerableExtensions.AddRange(list, items);

            Assert.IsTrue(items.SequenceEqual(list));

        }

        [Test]
        public void ThrowsExceptionIfNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.AddRange(null, new[] { 2, 3 }));
        }

        [Test]
        public void ThrowsExceptionIfNullItems()
        {
            var coll = new Collection<int>();
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.AddRange(coll, null));
        }
    }

    public class AddMissingRangeTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullSource()
        {
            Assert.Throws<ArgumentNullException>(() => { EnumerableExtensions.AddMissingRange(null, new int[2]); });
        }

        [Test]
        public void ThrowsExceptionIfNullTarget()
        {
            Assert.Throws<ArgumentNullException>(() => { EnumerableExtensions.AddMissingRange(new int[2], null); });
        }

        [Test]
        public void AddsMissingItemsCorrectly()
        {
            HashSet<int> target = new HashSet<int>() { 1, 2, 3 };

            target.AddMissingRange(new[] { 3, 4 });

            Assert.IsTrue(target.OrderBy(x => x).SequenceEqual(new[] { 1, 2, 3, 4 }));
        }
    }
}
