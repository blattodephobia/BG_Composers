using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.ComposerBuilderTests
{
    public class BuildComposerTest : TestFixtureBase
    {
        private readonly ComposerBuilder _builder = new ComposerBuilder(new ComposerMappers());

        [Test]
        public void BuildsComposer()
        {
            ComposerRelationalDto dto = new ComposerRelationalDto()
            {
                Articles = new[]
                {
                    new ArticleRelationalDto()
                    {
                        Language = "de-DE",
                        StorageId = new Guid(1, 1, 1, new byte[8]),
                    }
                },
                LocalizedNames = new[] { new NameRelationalDto() { FullName = "John Smith", Language = "de-DE" } },
                Profile = new ProfileRelationalDto() { ProfilePicture = new MediaTypeInfoRelationalDto() { MimeType = "image/jpeg" } },
            };

            Composer result = _builder.Build(dto);

            Assert.AreEqual(result.GetArticle(CultureInfo.GetCultureInfo("de-DE")).StorageId, dto.Articles.First().StorageId);
            Assert.AreEqual(result.Name[CultureInfo.GetCultureInfo("de-DE")].FullName, dto.LocalizedNames.First().FullName);
            Assert.AreEqual(result.Profile.ProfilePicture.MimeType, dto.Profile.ProfilePicture.MimeType);
        }
    }
}
