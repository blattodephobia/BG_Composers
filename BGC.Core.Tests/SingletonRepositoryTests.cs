using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests
{
    [TestFixture]
    public class SingletonRepositoryTests
    {
        class BaseEntity
        {

        }

        class DerivedEntity
        {

        }

        [Test]
        public void CreatesSingleInstanceOnFirstAccess()
        {
            SingletonRepository<BaseEntity> repo = new SingletonRepository<BaseEntity>();
        }
    }
}
