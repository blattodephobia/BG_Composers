using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    [TestFixture]
    public class TestFixtureBase
    {
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
        }
    }
}
