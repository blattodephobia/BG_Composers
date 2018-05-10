using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.IndexerTests
{
    public class GetTests : TestFixtureBase
    {
        [Test]
        public void CallsGetCallbackWhenGettingValue()
        {
            int getCallbacks = 0;
            int returnPseudoValue = 5;
            Func<int, int> getCallback = (int x) =>
            {
                getCallbacks++;
                return returnPseudoValue;
            };
            var indexer = new Indexer<int, int>(getCallback, (int x, int y) => { });

            int pseudoValue = indexer[1];

            Assert.AreEqual(1, getCallbacks);
            Assert.AreEqual(returnPseudoValue, pseudoValue);
        }
    }

    public class SetTests : TestFixtureBase
    {
        [Test]
        public void CallsSetCallbackWhenSettingValue()
        {
            int setCallbacks = 0;
            Action<int, int> setCallback = (int x, int y) =>
            {
                setCallbacks++;
            };
            var indexer = new Indexer<int, int>((int x) => 5, setCallback);

            indexer[1] = 2;

            Assert.AreEqual(1, setCallbacks);
        }
    }

    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsArgumentNullIfGetCallbackNull()
        {
            Action<int, int> setCallback = (int x, int y) => {};

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Indexer<int, int>(null, setCallback);
            });
        }

        [Test]
        public void ThrowsArgumentNullIfSetCallbackNull()
        {
            Func<int, int> getCallback = (int x) => 5;

            Assert.Throws<ArgumentNullException>(() =>
            {
                new Indexer<int, int>(getCallback, null);
            });
        }
    }
}
