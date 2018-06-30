using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Services.Tests.ComposerDataServiceTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullRepo()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerDataService(repo: null));
        }
    }

    public class GetAllComposersTests : TestFixtureBase
    {
        private ComposerDataService _svc;
        private readonly List<Composer> _composers = new List<Composer>();

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            _svc = new ComposerDataService(GetMockComposerRepository(_composers).Object);
        }

        public override void BeforeEachTest()
        {
            base.BeforeEachTest();

            _composers.Clear();
        }

        [Test]
        public void ReturnsAllComposersFromRepo()
        {
            _composers.AddRange(new[]
            {
                new Composer() { Id = new Guid(1, 0, 0, new byte[8]) },
                new Composer() { Id = new Guid(2, 0, 0, new byte[8]) },
                new Composer() { Id = new Guid(3, 0, 0, new byte[8]) },
                new Composer() { Id = new Guid(4, 0, 0, new byte[8]) },
            });

            List<Guid> ids1 = _svc.GetAllComposers().Select(c => c.Id).ToList();
            List<Guid> ids2 = _composers.Select(c => c.Id).ToList();

            ids1.Sort((x, y) => x.CompareTo(y));
            ids2.Sort((x, y) => x.CompareTo(y));

            Assert.IsTrue(ids1.SequenceEqual(ids2));
        }
    }

    public class AddOrUpdateTests : TestFixtureBase
    {
        [Test]
        public void SetsOrderToZeroWhenNoDuplicates()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer() { Id = new Guid(1, 0, 0, new byte[8]) },
                new Composer() { Id = new Guid(2, 3, 4, new byte[8]) }
            };

            for (int i = 0; i < mockRepo.Count; i++)
            {
                mockRepo[i].Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName(i.ToString(), CultureInfo.GetCultureInfo("en-US"));
            }

            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            svc.AddOrUpdate(new Composer() { Id = new Guid(5, 6, 7, new byte[8]) });

            Assert.AreEqual(0, mockRepo.Last().Order);
        }

        [Test]
        public void SetsOrderToOneWithOneDuplicateName_SingleCulture()
        {
            string duplicateName = "Rumpold Keltskin";
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>()
                    {
                        new ComposerName(duplicateName, "en-US"),
                        new ComposerName("asd", "bg-BG"),
                    },
                    DateAdded = new DateTime(2000, 1, 1)
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            var duplicateComposer = new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>()
                {
                    new ComposerName(duplicateName, "en-US"),
                    new ComposerName("oidjf", "bg-BG"),
                }
            };
            svc.AddOrUpdate(duplicateComposer);

            Assert.AreEqual(1, mockRepo.Last().Order, "Wrong order when there are duplicate names in only one of the supported locales.");
        }

        [Test]
        public void SetsOrderToOneWithOneDuplicateName_MoreCultures()
        {
            string duplicateName = "Rumpold Keltskin";
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>()
                    {
                        new ComposerName(duplicateName, "en-US"),
                        new ComposerName("asd", "bg-BG"),
                    },
                    DateAdded = new DateTime(2000, 1, 1)
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            var duplicateComposer = new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>()
                {
                    new ComposerName(duplicateName, "en-US"),
                    new ComposerName("asd", "bg-BG"),
                }
            };
            svc.AddOrUpdate(duplicateComposer);

            Assert.AreEqual(1, mockRepo.Last().Order, "Wrong order when there are duplicate names in all supported cultures.");
        }

        [Test]
        public void SetsOrderToOneWithMoreDuplicateNames_MixedCultures()
        {
            string duplicateName = "Rumpold Keltskin";
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>()
                    {
                        new ComposerName(duplicateName, "en-US"),
                        new ComposerName(duplicateName, "de-DE"),
                        new ComposerName("asd", "bg-BG"),
                    },
                    DateAdded = new DateTime(2000, 1, 1)
                },
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>()
                    {
                        new ComposerName(duplicateName, "en-US"),
                        new ComposerName("dkk", "de-DE"),
                        new ComposerName("asd", "bg-BG"),
                    },
                    DateAdded = new DateTime(2000, 1, 2)
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            var duplicateComposer = new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>()
                {
                    new ComposerName(duplicateName, "en-US"),
                    new ComposerName(duplicateName, "de-DE"),
                    new ComposerName("okl", "bg-BG"),
                }
            };
            svc.AddOrUpdate(duplicateComposer);

            Assert.AreEqual(2, duplicateComposer.Order, "Wrong order when there are duplicate names in some of the supported cultures.");
        }

        [Test]
        public void SetsNamesakePropertyCorrectly()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") },
                    DateAdded = new DateTime(2000, 1, 1)
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            svc.AddOrUpdate(new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
            });

            Assert.IsTrue(mockRepo.All(c => c.HasNamesakes));
        }

        [Test]
        public void LeavesOutNamesakePropertyIfNoDuplicateNames()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository().Object);

            svc.AddOrUpdate(new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>() { new ComposerName("Betty Boop", "en-US") }
            });

            Assert.IsFalse(mockRepo.All(c => c.HasNamesakes));
        }

        [Test]
        public void LeavesOutNamesakePropertyIfNoDuplicateNames_MixedNames()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") },
                    DateAdded = new DateTime(2000, 1, 1)
                },
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("Betty Boop", "en-US") },
                    DateAdded = new DateTime(2000, 1, 2)
                }
            };
            var svc = new ComposerDataService(GetMockComposerRepository(mockRepo).Object);

            svc.AddOrUpdate(new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
            });

            Assert.IsTrue(mockRepo
                .Where(c => c.Name[CultureInfo.GetCultureInfo("en-US")].FullName == "John Smith")
                .All(c => c.HasNamesakes));

            Assert.IsFalse(mockRepo
                .Where(c => c.Name[CultureInfo.GetCultureInfo("en-US")].FullName != "John Smith")
                .All(c => c.HasNamesakes));
        }
    }

    [TestFixture]
    public class SearchComposerNameTests
    {
        private static readonly List<Composer> ComposersRepo = new List<Composer>()
        {
            new Composer() { Id = Guid.NewGuid(), LocalizedNames = new List<ComposerName>()
            {
                new ComposerName(@"John Stamos", "en-US"),
                new ComposerName(@"John Stamos", "de-DE"),
                new ComposerName(@"Джон Стамос", "bg-BG"),
            }},

            new Composer() { Id = Guid.NewGuid(), LocalizedNames = new List<ComposerName>()
            {
                new ComposerName(@"Petar Stupel", "en-US"),
                new ComposerName(@"Petar Stupel", "de-DE"),
                new ComposerName(@"Петър Ступел", "bg-BG"),
            }},

            new Composer() { Id = Guid.NewGuid(), LocalizedNames = new List<ComposerName>()
            {
                new ComposerName(@"Pancho Vladigerov", "en-US"),
                new ComposerName(@"Pancho Vladigerov", "de-DE"),
                new ComposerName(@"Панчо Владигеров", "bg-BG"),
            }},

            new Composer() { Id = Guid.NewGuid(), LocalizedNames = new List<ComposerName>()
            {
                new ComposerName(@"John Atanasoff", "en-US"),
                new ComposerName(@"John Atanasoff", "de-DE"),
                new ComposerName(@"Джон Атанасов", "bg-BG"),
            }},

            new Composer() { Id = Guid.NewGuid(), LocalizedNames = new List<ComposerName>()
            {
                new ComposerName(@"Atanas Dalchev", "en-US"),
                new ComposerName(@"Atanas Dalchev", "de-DE"),
                new ComposerName(@"Атанас Далчев", "bg-BG"),
            }},
        };

        private readonly ComposerDataService _svc = new ComposerDataService(GetMockComposerRepository(ComposersRepo).Object);

        [Test]
        public void SearchComposer_SingleKeyword()
        {
            IEnumerable<string> results = _svc.Search("Atanas", new CultureInfo("en-US")).ToList().Select(result => result.Header).Distinct();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Atanas Dalchev", results.Single(header => header == "Atanas Dalchev"));
            Assert.AreEqual("John Atanasoff", results.Single(header => header == "John Atanasoff"));
        }

        [Test]
        public void SearchComposer_TwoKeywords()
        {
            IEnumerable<string> results = _svc.Search("Petar, Dalchev", new CultureInfo("de-DE")).ToList().Select(result => result.Header).Distinct();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Atanas Dalchev", results.Single(header => header == "Atanas Dalchev"));
            Assert.AreEqual("Petar Stupel", results.Single(header => header == "Petar Stupel"));
        }

        [Test]
        public void SearchComposer_TwoKeywords_SingleSearchPhrase()
        {
            List<SearchResult> results = _svc.Search("Petar Dalchev", new CultureInfo("en-US")).ToList();

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchComposer_ThreeKeywords_NoMatch()
        {
            List<SearchResult> results = _svc.Search("Ivan, Robert, Bach", new CultureInfo("en-US")).ToList();

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchComposer_ThreeKeywords_TwoSearchPhrases()
        {
            List<SearchResult> results = _svc.Search("Ivan, Панчо Владигеров", new CultureInfo("bg-BG")).ToList();

            Assert.AreEqual("Панчо Владигеров", results.First().Header);
        }

        [Test]
        public void SearchComposer_ThreeKeywords_TwoSearchPhrases_DifferentLocale()
        {
            List<SearchResult> results = _svc.Search("Ivan, Панчо Владигеров", new CultureInfo("en-US")).ToList();

            Assert.AreEqual("Pancho Vladigerov", results.First().Header);
        }

        [Test]
        public void DoesntInsertAlreadyExistingComposer()
        {
            var duplicateComposer = new Composer() { Id = ComposersRepo[0].Id };
            _svc.AddOrUpdate(duplicateComposer);

            Assert.AreNotSame(duplicateComposer, ComposersRepo[0]);
        }

        [Test]
        public void InsertsNewComposer()
        {
            var newComposer = new Composer() { Id = Guid.NewGuid() };
            _svc.AddOrUpdate(newComposer);

            Assert.IsTrue(ComposersRepo.Contains(newComposer));
        }
    }

    public class GetNamesTests : TestFixtureBase
    {
        private ComposerDataService _svc;
        private readonly List<Composer> _composers = new List<Composer>();

        public string[] _enNames = new[] { "One", "Two", "Three", "Four" };
        public string[] _bgNames = new[] { "Едно", "Две", "Три", "Четири" };

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            _svc = new ComposerDataService(GetMockComposerRepository(_composers).Object);

            _composers.AddRange(Enumerable.Range(0, 4).Select(i =>
            {
                Composer fakeComposer = new Composer();
                fakeComposer.Name[CultureInfo.GetCultureInfo("en-US")] = new ComposerName(_enNames[i], CultureInfo.GetCultureInfo("en-US"));
                fakeComposer.Name[CultureInfo.GetCultureInfo("bg-BG")] = new ComposerName(_bgNames[i], CultureInfo.GetCultureInfo("bg-BG"));

                return fakeComposer;
            }));
        }

        [Test]
        public void ThrowsExceptionIfNullLocale()
        {
            Assert.Throws<ArgumentNullException>(() => _svc.GetNames(null));
        }

        [Test]
        public void GetsAllNamesWithGivenLocale()
        {
            HashSet<string> names = new HashSet<string>(_svc.GetNames(CultureInfo.GetCultureInfo("bg-BG")).Select(n => n.FullName));

            Assert.AreEqual(_bgNames.Length, names.Count);
            Assert.IsTrue(_bgNames.All(s => names.Contains(s)));
        }
    }

    public class FindComposerTests : TestFixtureBase
    {
        private readonly List<Composer> _composers;
        private readonly ComposerDataService _svc;

        public FindComposerTests()
        {
            _composers = new List<Composer>()
            {
                new Composer() { Id = new Guid(1, 2, 3, new byte[8]) },
                new Composer() { Id = new Guid(4, 5, 6, new byte[8]) },
            };

            _svc = new ComposerDataService(GetMockComposerRepository(_composers).Object);
        }

        [Test]
        public void FindsComposer()
        {
            Composer entity = _svc.FindComposer(_composers[0].Id);

            Assert.AreEqual(_composers[0].Id, entity.Id);
        }

        [Test]
        public void ReturnsNullIfComposerNotFound()
        {
            Composer entity = _svc.FindComposer(new Guid(9, 9, 9, new byte[8]));

            Assert.IsNull(entity);
        }
    }
}
