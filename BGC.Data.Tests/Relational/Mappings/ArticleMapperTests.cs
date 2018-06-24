using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.ArticleMapperTests
{
    public class CopyDataTests : TestFixtureBase
    {
        private readonly ArticlePropertyMapper _mapper = new ArticlePropertyMapper();

        [Test]
        public void CopiesDataCorrectly()
        {
            var article = new ComposerArticle();
            article.CreatedUtc = new DateTime(1970, 2, 2);
            article.IsArchived = true;
            article.Language = new CultureInfo("en-US");
            article.StorageId = new Guid(2, 1, 0, new byte[8]);

            var dto = _mapper.CopyData(article, new ArticleRelationalDto());            

            Assert.AreEqual(dto.CreatedUtc, article.CreatedUtc);
            Assert.AreEqual(dto.IsArchived, article.IsArchived);
            Assert.AreEqual(dto.Language, article.Language.Name);
            Assert.AreEqual(dto.StorageId, article.StorageId);
        }
    }

    public class ToEntityTests : TestFixtureBase
    {
        private readonly ComposerPropertyMapper _mapper = new ComposerPropertyMapper();

        [Test]
        public void ThrowsExceptionIfNullSource()
        {
            Assert.Throws<ArgumentNullException>(() => _mapper.CopyData(null, new Composer()));
        }

        [Test]
        public void CreatesCorrectEntity()
        {
            var dto = new ComposerRelationalDto()
            {
                Id = new Guid(5, 1, 0, new byte[8]),
                DateOfBirth = new DateTime(1970, 2, 2),
                DateOfDeath = new DateTime(2070, 2, 2),
                Order = 3,
            };

            Composer entity = _mapper.CopyData(dto, new Composer());

            Assert.AreEqual(dto.Id, entity.Id);
            Assert.AreEqual(dto.DateOfBirth, entity.DateOfBirth);
            Assert.AreEqual(dto.DateOfDeath, entity.DateOfDeath);
            Assert.AreEqual(dto.Order, entity.Order);
        }
    }
}
