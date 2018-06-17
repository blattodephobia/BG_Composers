using BGC.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.ComposerBreakdownTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullMappersObject()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerBreakdown(null));
        }
    }

    public class BreakdownTests : TestFixtureBase
    {
        private readonly ComposerBreakdown _breakdown = new ComposerBreakdown(new ComposerMappers());
        
        [Test]
        public void BreakdownNames()
        {
            Composer c = new Composer();
            var name1 = new ComposerName("John Smith", "en-US");
            var name2 = new ComposerName("John Smith", "de-DE");
            c.Name[name1.Language] = name1;
            c.Name[name2.Language] = name2;

            List<NameRelationalDto> names = _breakdown.Breakdown(c).OfType<NameRelationalDto>().ToList();

            NameRelationalDto name1Dto = names.FirstOrDefault(n => n.Language == name1.Language.Name);
            NameRelationalDto name2Dto = names.FirstOrDefault(n => n.Language == name2.Language.Name);
            
            Assert.IsNotNull(name1Dto, "Name wasn't copied at all.");
            Assert.AreEqual(name1.FullName, name1Dto.FullName);
            Assert.AreEqual(name1Dto.Composer.Id, c.Id);

            Assert.IsNotNull(name2Dto, "Name wasn't copied at all.");
            Assert.AreEqual(name2.FullName, name2Dto.FullName);
            Assert.AreEqual(name2Dto.Composer.Id, c.Id);
        }

        [Test]
        public void BreakdownArticles()
        {
            Composer c = new Composer();
            var article1 = new ComposerArticle(c, new ComposerName("John Smith", "en-US"), CultureInfo.GetCultureInfo("en-US"));
            var article2 = new ComposerArticle(c, new ComposerName("John Smith", "de-DE"), CultureInfo.GetCultureInfo("de-DE"));
            c.AddArticle(article1);
            c.AddArticle(article2);

            List<ArticleRelationalDto> articles = _breakdown.Breakdown(c).OfType<ArticleRelationalDto>().ToList();

            ArticleRelationalDto article1Dto = articles.FirstOrDefault(a => a.Language == article1.Language.Name);
            ArticleRelationalDto article2Dto = articles.FirstOrDefault(a => a.Language == article2.Language.Name);

            Assert.IsNotNull(article1Dto, "Article wasn't copied at all.");
            Assert.AreEqual(article1.StorageId, article1Dto.StorageId);
            Assert.AreEqual(article1.Composer.Id, article1Dto.Composer.Id);

            Assert.IsNotNull(article2Dto, "Article wasn't copied at all.");
            Assert.AreEqual(article2.StorageId, article2Dto.StorageId);
            Assert.AreEqual(article2.Composer.Id, article2Dto.Composer.Id);
        }

        [Test]
        public void BreakdownProfile()
        {
            Composer c = new Composer();
            var profile = new ComposerProfile();
            c.Profile = profile;
            var profilePic = new MediaTypeInfo("pic.jpg", MediaTypeNames.Image.Jpeg) { StorageId = new Guid(1, 0, 0, new byte[8]) };
            var otherMedia = new MediaTypeInfo("demo.mp3", MediaTypeNames.Application.Octet) { StorageId = new Guid(2, 0, 0, new byte[8]) };
            profile.ProfilePicture = profilePic;
            profile.Media.Add(otherMedia);

            List<MediaTypeInfoRelationalDto> media = _breakdown.Breakdown(c).OfType<MediaTypeInfoRelationalDto>().ToList();
            var profilePicDto = media.FirstOrDefault(m => m.StorageId == profilePic.StorageId);
            var otherMediaDto = media.FirstOrDefault(m => m.StorageId == otherMedia.StorageId);

            Assert.IsNotNull(profilePicDto, "Profile picture wasn't copied at all.");
            Assert.AreEqual(profilePic.StorageId, profilePicDto.StorageId);

            Assert.IsNotNull(otherMediaDto, "Profile picture wasn't copied at all.");
            Assert.AreEqual(otherMedia.StorageId, otherMediaDto.StorageId);
        }
    }
}
