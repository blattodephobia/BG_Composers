using BGC.Web.Areas.Administration.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Web.Tests.AdministrationArea.ViewModels.GlossaryEntryViewModelTests
{
    [TestFixture]
    public class GetDefinitionsInLocaleTests
    {
        [Test]
        public void GetsCorrectSingleDefinition()
        {
            var vm = new GlossaryEntryViewModel()
            {
                Definitions = new List<GlossaryDefinitionViewModel>()
                {
                    new GlossaryDefinitionViewModel() { LocaleCode = "en-US", Definition = "english" },
                    new GlossaryDefinitionViewModel() { LocaleCode = "de-DE", Definition = "Deutsch" }
                }
            };

            Assert.AreEqual("Deutsch", vm.GetDefinitionsInLocale(new CultureInfo("de-DE")).First().Definition);
        }

        [Test]
        public void GetsCorrectMultipleDefinitions()
        {
            var vm = new GlossaryEntryViewModel()
            {
                Definitions = new List<GlossaryDefinitionViewModel>()
                {
                    new GlossaryDefinitionViewModel() { LocaleCode = "en-US", Definition = "english" },
                    new GlossaryDefinitionViewModel() { LocaleCode = "de-DE", Definition = "Deutsch" },
                    new GlossaryDefinitionViewModel() { LocaleCode = "bg-BG", Definition = "български" }
                }
            };

            var expected = new string[] { "Deutsch", "български" }.OrderBy(x => x);
            var actual = vm.GetDefinitionsInLocale(new CultureInfo("de-DE"), new CultureInfo("bg-BG")).Select(def => def.Definition).OrderBy(x => x);
            Assert.IsTrue(expected.SequenceEqual(actual));
        }
    }
}
