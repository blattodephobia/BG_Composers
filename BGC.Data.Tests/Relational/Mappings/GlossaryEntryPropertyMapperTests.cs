using BGC.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings
{
    public class GlossaryEntryPropertyMapperTests : TestFixtureBase
    {
        [Test]
        public void MapsIdPropertyCorrectly()
        {
            var mapper = new GlossaryEntryPropertyMapper();

            var dto = new GlossaryEntryRelationalDto() { DomainId = new Guid(1, 2, 3, new byte[8]) };
            GlossaryEntry entity = mapper.CopyData(dto, new GlossaryEntry());

            Assert.AreEqual(dto.DomainId, entity.Id);
        }
    }
}
