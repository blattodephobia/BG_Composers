using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Utilities.Tests.LambdaTypeSubstitutionTests
{
    public class ChangeLambdaTypeTests : TestFixtureBase
    {
        private interface IMockDto
        {
            string Name { get; set; }
        }

        private class MockDto : IMockDto
        {
            public string Name { get; set; }
        }

        [Test]
        public void ConvertsLambdaSuccessfully()
        {
            Expression<Func<IMockDto, bool>> test = f => f.Name.Length != 0;
            var conv = new LambdaTypeSubstitution<IMockDto, MockDto>();

            Assert.IsTrue(conv.ChangeLambdaType(test).Compile().Invoke(new MockDto() { Name = "asd" }));
        }
    }
}
