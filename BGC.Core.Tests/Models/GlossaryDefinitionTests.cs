using BGC.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models.GlossaryDefinitionTests
{
    [TestFixture]
    public class CtorTests
    {
        [Test]
        public void ThrowsExceptionIfNullLanguage()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GlossaryDefinition(null, "definition");
            });
        }

        [Test]
        public void ThrowsExceptionIfNullDefinition()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new GlossaryDefinition(new CultureInfo("en-US"), null);
            });

        }

        [Test]
        public void ThrowsExceptionIfEmptyDefinition()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                new GlossaryDefinition(new CultureInfo("en-US"), "");
            });
        }

        [Test]
        public void SetsPropertiesCorrectly()
        {
            var def = new GlossaryDefinition(CultureInfo.GetCultureInfo("en-US"), "Hello world");

            Assert.AreEqual("en-US", def.LanguageInternal);
            Assert.AreEqual(CultureInfo.GetCultureInfo("en-US"), def.Language);
            Assert.AreEqual("Hello world", def.Definition);
        }
    }

    [TestFixture]
    public class LanguageTests
    {
        [Test]
        public void ThrowsExceptionIfLanguageNotSupported()
        {
            var def = new GlossaryDefinition(new CultureInfo("de-DE"), "Deutsch");

            Assert.Throws<CultureNotFoundException>(() => def.LanguageInternal = "z3-45");
        }

        [Test]
        public void SetsLanguageCorrectly()
        {
            var def = new GlossaryDefinition(new CultureInfo("de-DE"), "Deutsch");

            def.Language = new CultureInfo("en-US");
            Assert.AreEqual("en-US", def.LanguageInternal);
        }
    }
}
