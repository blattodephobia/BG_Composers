using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Utilities.Tests
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
}
