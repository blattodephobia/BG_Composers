using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestUtils.MockUtilities;

namespace BGC.Services.Tests
{
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
