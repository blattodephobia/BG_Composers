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
using static TestUtils.MockUtilities;

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

        [Test]
        public void DoesntReturnDuplicates()
        {
            ComposerRelationalDto composer1 = new ComposerRelationalDto() { Id = new Guid(1, 0, 0, new byte[8]) };
            ComposerRelationalDto composer2 = new ComposerRelationalDto() { Id = new Guid(2, 0, 0, new byte[8]) };

            NameRelationalDto name1_a = new NameRelationalDto() { Composer_Id = composer1.Id, Composer = composer1, FullName = "Georgi Popov", Language = "en-US" };
            NameRelationalDto name1_b = new NameRelationalDto() { Composer_Id = composer1.Id, Composer = composer1, FullName = "Георги Попов", Language = "bg-BG" };
            NameRelationalDto name2_a = new NameRelationalDto() { Composer_Id = composer2.Id, Composer = composer2, FullName = "Georgi Popoff", Language = "en-US" };
            NameRelationalDto name2_b = new NameRelationalDto() { Composer_Id = composer2.Id, Composer = composer2, FullName = "Георги Попов", Language = "bg-BG" };

            _composersList.AddRange(new[] { composer1, composer2 });
            _namesList.AddRange(new[] { name1_a, name1_b, name2_a, name2_b });

            IEnumerable<Composer> searchResult = _composersRepository.Find(name => name.FullName == "Георги Попов" || name.FullName == "Georgi Popov");

            Assert.AreEqual(2, searchResult.Count());
        }
    }

    public class AddOrUpdateTests : TestFixtureBase
    {
        private ComposersRepository _repo;
        private bool _throwException = false;

        private DbSet CustomDbSetFactory(Type t)
        {
            if (_throwException)
            {
                throw new Exception();
            }
            else
            {
                return DefaultSetFactory.Invoke(t);
            }
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            _throwException = false;
        }

        public override void OneTimeSetUp()
        {
            Mock<DbContext> ctx = GetMockDbContext();
            ctx.Setup(x => x.Set(It.IsAny<Type>())).Returns<Type>(CustomDbSetFactory);

            _repo = new ComposersRepository(new ComposerTypeMapper(new ComposerMappers(), new MockDtoFactory()), new ComposerPropertyMapper(), ctx.Object);
        }

        [Test]
        public void SetsDateAddedCorrectly()
        {
            Composer entity = new Composer();
            DateTime beforeUpdate = DateTime.UtcNow;

            _repo.AddOrUpdate(entity);

            Assert.GreaterOrEqual(entity.DateAdded, beforeUpdate);
        }

        [Test]
        public void DoesntSetDateAddedIfException()
        {
            Composer entity = new Composer() { DateAdded = DateTime.MinValue };
            _throwException = true;

            Assert.Throws<Exception>(() => _repo.AddOrUpdate(entity));

            Assert.AreEqual(DateTime.MinValue, entity.DateAdded);
        }
    }
}
