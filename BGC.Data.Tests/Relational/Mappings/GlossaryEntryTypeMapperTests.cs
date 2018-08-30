using BGC.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.GlossaryEntryTypeMapperTests
{
    public class BuildTests : TestFixtureBase
    {
        [Test]
        public void MapsCorrectCountOfDefinitions()
        {
            GlossaryEntryRelationalDto dto = new GlossaryEntryRelationalDto()
            {
                Translations = new List<GlossaryDefinitionRelationalDto>()
                {
                    new GlossaryDefinitionRelationalDto() { Language = "en-US", DefinitionTranslation = "a", TermTranslation = "a" },
                    new GlossaryDefinitionRelationalDto() { Language = "de-DE", DefinitionTranslation = "b", TermTranslation = "b" },
                    new GlossaryDefinitionRelationalDto() { Language = "bg-BG", DefinitionTranslation = "c", TermTranslation = "c" },
                }
            };

            var typeMapper = new GlossaryEntryTypeMapper(new GlossaryEntryPropertyMapper(), new MockDtoFactory());

            GlossaryEntry entity = typeMapper.Build(dto);

            Assert.AreEqual(dto.Translations.Count, entity.Definitions.Count);
        }
    }
}
