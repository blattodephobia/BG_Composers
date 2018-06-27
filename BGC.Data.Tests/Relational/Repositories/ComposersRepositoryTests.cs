using BGC.Core;
using BGC.Data.Relational.Mappings;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Repositories.ComposersRepositoryTests
{
    public class FindTests : TestFixtureBase
    {
        private ComposersRepository _composersRepository;
        private List<ComposerRelationalDto> _composersList;
        private List<NameRelationalDto> _namesList;

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();

            _composersList = new List<ComposerRelationalDto>();
            _namesList = new List<NameRelationalDto>();

            var mockContext = new Mock<DbContext>();
            DbSet<ComposerRelationalDto> composersSet = MockUtilities.GetMockDbSet(_composersList).Object;
            DbSet<NameRelationalDto> namesSet = MockUtilities.GetMockDbSet(_namesList).Object;
            mockContext.Setup(d => d.Set<ComposerRelationalDto>()).Returns(composersSet);
            mockContext.Setup(d => d.Set<NameRelationalDto>()).Returns(namesSet);
            _composersRepository = new ComposersRepository(
                typeMapper: new ComposerTypeMapper(new ComposerMappers(), new MockDtoFactory()),
                propertyMapper: new ComposerPropertyMapper(),
                context: mockContext.Object);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            _composersList.Clear();
            _namesList.Clear();
        }

        [Test]
        public void ThrowsExceptionIfNullSelector()
        {
            Assert.Throws<ArgumentNullException>(() => _composersRepository.Find(null));
        }

        [Test]
        public void FindsCorrectComposer()
        {
            string fullName = "John Addams";
            ComposerRelationalDto c = new ComposerRelationalDto();
            NameRelationalDto name = new NameRelationalDto() { FullName = fullName, Composer = c, Language = CultureInfo.GetCultureInfo("en-US").Name };
            c.LocalizedNames.Add(name);

            _composersList.Add(c);
            _namesList.Add(name);

            IEnumerable<Composer> searchResult = _composersRepository.Find(d => d.FullName == fullName);
            bool hasMatches = searchResult.Any(sr => sr.Name.All().Any(n => n.Value.FullName == fullName));

            Assert.IsTrue(hasMatches);
        }
    }
}
