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

            Guid Id { get; set; }

            string OtherId { get; set; }
        }

        private class MockDto : IMockDto
        {
            public string Name { get; set; }

            public int Id { get; set; }

            Guid IMockDto.Id { get; set; }

            string IMockDto.OtherId
            {
                get
                {
                    return Name;
                }

                set
                {
                    Name = value;
                }
            }
        }

        private readonly LambdaTypeSubstitution<IMockDto, MockDto> _conv;

        public ChangeLambdaTypeTests()
        {
            _conv = new LambdaTypeSubstitution<IMockDto, MockDto>();
        }

        [Test]
        public void ConvertsLambdaSuccessfully()
        {
            Expression<Func<IMockDto, bool>> test = f => f.Name.Length != 0;

            Assert.IsTrue(_conv.ChangeLambdaType(test).Compile().Invoke(new MockDto() { Name = "asd" }));
        }

        [Test]
        public void ConvertsWithExplicitInterfaceDeclaration_SamePropertyName()
        {
            Guid val = new Guid(7, 1, 1, new byte[8]);
            Expression<Func<IMockDto, bool>> test = f => f.Id == val;

            var dto = new MockDto();
            (dto as IMockDto).Id = val;
            Assert.IsTrue(_conv.ChangeLambdaType(test).Compile().Invoke(dto));
        }

        [Test]
        public void ConvertsWithExplicitInterfaceDeclaration_DifferentPropertyName()
        {
            string val = "hello";
            Expression<Func<IMockDto, bool>> test = f => f.OtherId == val;

            var dto = new MockDto();
            (dto as IMockDto).OtherId = val;
            Assert.IsTrue(_conv.ChangeLambdaType(test).Compile().Invoke(dto));
        }

        [Test]
        public void SubstitutesParamReferencesOnly()
        {
            IMockDto val = new MockDto() { Name = "hello" };
            Expression<Func<IMockDto, bool>> test = f => f.OtherId == val.OtherId;

            var dto = new MockDto();
            (dto as IMockDto).OtherId = val.OtherId;

            Assert.IsTrue(_conv.ChangeLambdaType(test).Compile().Invoke(dto));
        }
    }
}
