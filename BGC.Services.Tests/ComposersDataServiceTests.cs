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
    public class AddOrUpdateTests : TestFixtureBase
    {
        [Test]
        public void SetsOrderToZeroWhenNoDuplicates()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer() { Id = Guid.NewGuid() }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(new List<ComposerName>()).Object);

            svc.AddOrUpdate(new Composer());

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
                    }
                }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

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
                    }
                }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

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
                    }
                },
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>()
                    {
                        new ComposerName(duplicateName, "en-US"),
                        new ComposerName("dkk", "de-DE"),
                        new ComposerName("asd", "bg-BG"),
                    }
                }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

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

            Assert.AreEqual(2, mockRepo.Last().Order, "Wrong order when there are duplicate names in some of the supported cultures.");
        }

        [Test]
        public void SetsNamesakePropertyCorrectly()
        {
            List<Composer> mockRepo = new List<Composer>()
            {
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
                }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

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
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

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
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
                },
                new Composer()
                {
                    Id = Guid.NewGuid(),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("Betty Boop", "en-US") }
                }
            };
            var svc = new ComposerDataService(GetMockRepository(mockRepo).Object, GetMockRepository(mockRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);

            svc.AddOrUpdate(new Composer()
            {
                Id = Guid.NewGuid(),
                LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") }
            });

            Assert.IsTrue(mockRepo
                .Where(c => c.GetName(CultureInfo.GetCultureInfo("en-US")).FullName == "John Smith")
                .All(c => c.HasNamesakes));

            Assert.IsFalse(mockRepo
                .Where(c => c.GetName(CultureInfo.GetCultureInfo("en-US")).FullName != "John Smith")
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

        [Test]
        public void SearchComposer_SingleKeyword()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            IEnumerable<string> results = svc.Search("Atanas").ToList().Select(result => result.Header).Distinct();
            
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Atanas Dalchev", results.Single(header => header == "Atanas Dalchev"));
            Assert.AreEqual("John Atanasoff", results.Single(header => header == "John Atanasoff"));
        }

        [Test]
        public void SearchComposer_TwoKeywords()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            IEnumerable<string> results = svc.Search("Petar, Dalchev").ToList().Select(result => result.Header).Distinct();

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Atanas Dalchev", results.Single(header => header == "Atanas Dalchev"));
            Assert.AreEqual("Petar Stupel", results.Single(header => header == "Petar Stupel"));
        }

        [Test]
        public void SearchComposer_TwoKeywords_SingleSearchPhrase()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            List<SearchResult> results = svc.Search("Petar Dalchev").ToList();

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchComposer_ThreeKeywords_NoMatch()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            List<SearchResult> results = svc.Search("Ivan, Robert, Bach").ToList();

            Assert.AreEqual(0, results.Count);
        }

        [Test]
        public void SearchComposer_ThreeKeywords_TwoSearchPhrases()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            List<SearchResult> results = svc.Search("Ivan, Панчо Владигеров").ToList();

            Assert.AreEqual("Панчо Владигеров", results.First().Header);
        }

        [Test]
        public void DoesntInsertAlreadyExistingComposer()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            var duplicateComposer = new Composer() { Id = ComposersRepo[0].Id };
            svc.AddOrUpdate(duplicateComposer);

            Assert.AreNotSame(duplicateComposer, ComposersRepo[0]);
        }

        [Test]
        public void InsertsNewComposer()
        {
            var svc = new ComposerDataService(GetMockRepository(ComposersRepo).Object, GetMockRepository(ComposersRepo.SelectMany(c => c.LocalizedNames).ToList()).Object);
            var newComposer = new Composer() { Id = Guid.NewGuid() };
            svc.AddOrUpdate(newComposer);

            Assert.IsTrue(ComposersRepo.Contains(newComposer));
        }
    }
}
