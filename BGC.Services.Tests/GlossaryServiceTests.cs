using BGC.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestUtils.MockUtilities;

namespace BGC.Services.Tests.GlossaryServiceTests
{
    [TestFixture]
    public class AddOrUpdateTest
    {
        [Test]
        public void AddsEntry()
        {
            var store = new List<GlossaryEntry>();
            var svc = new GlossaryService(GetMockRepository(store).Object);

            var newEntry = new GlossaryEntry();
            svc.AddOrUpdate(newEntry);

            Assert.AreSame(newEntry, store.Single());
        }

        [Test]
        public void UpdatesExistingEntry()
        {
            var store = new List<GlossaryEntry>();
            var svc = new GlossaryService(GetMockRepository(store).Object);

            var oldDefinitions = new List<GlossaryDefinition>()
            {
                new GlossaryDefinition(CultureInfo.GetCultureInfo("en-US"), "OLD", "term"),
                new GlossaryDefinition(CultureInfo.GetCultureInfo("de-DE"), "ALT", "term"),
            };

            var newDefinitions = new List<GlossaryDefinition>()
            {
                new GlossaryDefinition(CultureInfo.GetCultureInfo("en-US"), "NEW", "term"),
                new GlossaryDefinition(CultureInfo.GetCultureInfo("de-DE"), "NEU", "term"),
            };

            var commonId = new Guid("01234567-0004-0004-0004-123456789ABC");
            var existingEntry = new GlossaryEntry() { Id = commonId, Definitions = oldDefinitions };
            var editedEntry = new GlossaryEntry() { Id = commonId, Definitions = newDefinitions };

            store.Add(existingEntry);

            svc.AddOrUpdate(editedEntry);

            Assert.AreEqual(1, store.Count);

            IEnumerable<string> expectedDefinitions = newDefinitions.Select(def => def.Definition).OrderBy(x => x);
            IEnumerable<string> actualDefinitions = store.First().Definitions.Select(def => def.Definition).OrderBy(x => x);
            Assert.IsTrue(expectedDefinitions.SequenceEqual(actualDefinitions));
        }

        [Test]
        public void ThrowsExceptionIfNullEntry()
        {
            var store = new List<GlossaryEntry>();
            var svc = new GlossaryService(GetMockRepository(store).Object);

            Assert.Throws<ArgumentNullException>(() =>
            {
                svc.AddOrUpdate(null);
            });
        }
    }
}
