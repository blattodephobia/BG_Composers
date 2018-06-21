﻿using BGC.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.DomainBreakdownTests
{
    public class BreakdownTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullEntity()
        {
            var mock = new Mock<DomainBreakdownBase<Composer>>(new MockDtoFactory());

            Assert.Throws<ArgumentNullException>(() => mock.Object.Breakdown(null));
        }
    }
}
