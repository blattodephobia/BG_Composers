using BGC.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Tests.Models
{
    [TestFixture]
    public class GetDefinitionTests
    {
        [Test]
        public void ThrowsExceptionIfNullArgument()
        {
            var entry = new GlossaryEntry();
            Assert.Throws<ArgumentNullException>(() =>
            {
                entry.GetDefinition(null);
            });
        }

        [Test]
        public void GetsCorrectDefintion()
        {
            var entry = new GlossaryEntry()
            {
                Definitions = new List<GlossaryDefinition>()
                {
                    new GlossaryDefinition(new CultureInfo("en-US"), "english"),
                    new GlossaryDefinition(new CultureInfo("de-DE"), "Deutsch"),
                }
            };

            Assert.AreEqual("Deutsch", entry.GetDefinition(new CultureInfo("de-DE")).Definition);
        }
    }
}
