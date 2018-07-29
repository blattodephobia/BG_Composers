using BGC.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TestUtils;

namespace BGC.Data.Relational.Mappings.ComposerTypeMapperTests
{
    public class CtorTests : TestFixtureBase
    {
        [Test]
        public void ThrowsExceptionIfNullMappersObject()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerTypeMapper(null, new Mock<IDtoFactory>().Object));
        }

        [Test]
        public void ThrowsExceptionIfNullDtoFactory()
        {
            Assert.Throws<ArgumentNullException>(() => new ComposerTypeMapper(new ComposerMappers(), null));
        }
    }

    public class BuildDtoTests : TestFixtureBase
    {
        private readonly ComposerTypeMapper _standardBreakdown;
        private readonly Func<object, object, object> _defaultIntermediateCallback;
        private readonly MockDtoFactory _dtoFactory;

        public BuildDtoTests()
        {
            _defaultIntermediateCallback = (p, d) =>
            {
                if ((p is ComposerRelationalDto) && (d is MediaTypeInfoRelationalDto))
                {
                    return ComposerMediaRelationalDto.Create(p as ComposerRelationalDto, d as MediaTypeInfoRelationalDto);
                }
                else
                {
                    throw new NotSupportedException();
                }
            };
            _dtoFactory = new MockDtoFactory()
            {
                IntermediateDtoCallback = _defaultIntermediateCallback
            };
            _standardBreakdown = new ComposerTypeMapper(new ComposerMappers(), _dtoFactory);
        }
        
        [Test]
        public void MapNames()
        {
            Composer c = new Composer();
            var name1 = new ComposerName("John Smith", "en-US");
            var name2 = new ComposerName("John Smith", "de-DE");
            c.Name[name1.Language] = name1;
            c.Name[name2.Language] = name2;

            ICollection<NameRelationalDto> names = _standardBreakdown.BuildDto(c).LocalizedNames;

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
        public void MapArticles()
        {
            Composer c = new Composer();
            var article1 = new ComposerArticle(c, new ComposerName("John Smith", "en-US"), CultureInfo.GetCultureInfo("en-US"));
            var article2 = new ComposerArticle(c, new ComposerName("John Smith", "de-DE"), CultureInfo.GetCultureInfo("de-DE"));
            c.AddArticle(article1);
            c.AddArticle(article2);

            ICollection<ArticleRelationalDto> articles = _standardBreakdown.BuildDto(c).Articles;

            ArticleRelationalDto article1Dto = articles.FirstOrDefault(a => a.Language == article1.Language.Name);
            ArticleRelationalDto article2Dto = articles.FirstOrDefault(a => a.Language == article2.Language.Name);

            Assert.IsNotNull(article1Dto, "Article 1 wasn't copied at all.");
            Assert.AreEqual(article1.StorageId, article1Dto.StorageId);
            Assert.AreEqual(article1.Composer.Id, article1Dto.Composer.Id);

            Assert.IsNotNull(article2Dto, "Article 2 wasn't copied at all.");
            Assert.AreEqual(article2.StorageId, article2Dto.StorageId);
            Assert.AreEqual(article2.Composer.Id, article2Dto.Composer.Id);
        }

        [Test]
        public void MapProfile()
        {
            Composer c = new Composer();
            var profile = new ComposerProfile();
            c.Profile = profile;
            var profilePic = new MediaTypeInfo("pic.jpg", MediaTypeNames.Image.Jpeg) { StorageId = new Guid(1, 0, 0, new byte[8]) };
            var otherMedia = new MediaTypeInfo("demo.mp3", MediaTypeNames.Application.Octet) { StorageId = new Guid(2, 0, 0, new byte[8]) };
            profile.ProfilePicture = profilePic;
            profile.Media.Add(otherMedia);

            ComposerRelationalDto dto = _standardBreakdown.BuildDto(c);
            ICollection<ComposerMediaRelationalDto> media = dto.Media;
            MediaTypeInfoRelationalDto profilePicDto = dto.ProfilePicture;
            MediaTypeInfoRelationalDto otherMediaDto = media.FirstOrDefault(m => m.MediaEntry.StorageId == otherMedia.StorageId).MediaEntry;

            Assert.IsNotNull(profilePicDto, "Profile picture wasn't copied at all.");
            Assert.AreEqual(profilePic.StorageId, profilePicDto.StorageId);

            Assert.IsNotNull(otherMediaDto, "Music wasn't copied at all.");
            Assert.AreEqual(otherMedia.StorageId, otherMediaDto.StorageId);
        }

        [Test]
        public void SetsComposerPrincipalOnArticles()
        {
            Composer c = new Composer() { Id = new Guid(2, 2, 2, new byte[8]) };
            var article1 = new ComposerArticle(c, new ComposerName("John Smith", "en-US"), CultureInfo.GetCultureInfo("en-US"));
            var article2 = new ComposerArticle(c, new ComposerName("John Smith", "de-DE"), CultureInfo.GetCultureInfo("de-DE"));
            c.AddArticle(article1);
            c.AddArticle(article2);

            ICollection<ArticleRelationalDto> dtos = _standardBreakdown.BuildDto(c).Articles;

            Assert.AreEqual(2, dtos.Count);
            Assert.IsTrue(dtos.All(d => d.Composer != null));
        }

        [Test]
        public void SetsComposerPrincipalOnNames()
        {
            Composer c = new Composer() { Id = new Guid(3, 3, 3, new byte[8]) };
            var name1 = new ComposerName("John Smith", "en-US");
            var name2 = new ComposerName("John Smith", "de-DE");
            c.Name[name1.Language] = name1;
            c.Name[name2.Language] = name2;

            ICollection<NameRelationalDto> names = _standardBreakdown.BuildDto(c).LocalizedNames;

            Assert.AreEqual(2, names.Count);
            Assert.IsTrue(names.All(n => n.Composer!= null));
        }
        
        [Test]
        public void MapsMedia()
        {
            Composer c = new Composer() { Id = new Guid(4, 4, 4, new byte[8]) };
            var profile = new ComposerProfile();
            c.Profile = profile;

            var profilePic = new MediaTypeInfo("pic.jpg", MediaTypeNames.Image.Jpeg) { StorageId = new Guid(1, 0, 0, new byte[8]) };
            var otherMedia = new MediaTypeInfo("demo.mp3", MediaTypeNames.Application.Octet) { StorageId = new Guid(2, 0, 0, new byte[8]) };
            profile.ProfilePicture = profilePic;
            profile.Media.Add(otherMedia);

            ComposerRelationalDto composer = _standardBreakdown.BuildDto(c);
            ICollection<ComposerMediaRelationalDto> media = composer.Media;

            Assert.AreEqual(profilePic.StorageId, composer.ProfilePicture.StorageId);
            Assert.AreEqual(1, media.Count(m => m.MediaEntry.StorageId == otherMedia.StorageId && m.MediaEntry.OriginalFileName == otherMedia.OriginalFileName));
        }

        [Test]
        public void RemovesMissingNonArticleItems()
        {
            Mock<MockDtoFactory> factory = new Mock<MockDtoFactory>() { CallBase = true };
            factory.SetupAllProperties();
            factory.Object.IntermediateDtoCallback = _defaultIntermediateCallback;
            ComposerTypeMapper mapper = new ComposerTypeMapper(new ComposerMappers(), factory.Object);
            Guid[] idPool = new Guid[] { new Guid(1, 2, 3, new byte[8]), new Guid(4, 5, 6, new byte[8]), new Guid(7, 8, 9, new byte[8]) };
            ComposerRelationalDto dto = new ComposerRelationalDto();
            dto.Media = new List<ComposerMediaRelationalDto>()
            {
                ComposerMediaRelationalDto.Create(dto, new MediaTypeInfoRelationalDto() { StorageId = idPool[0] }),
                ComposerMediaRelationalDto.Create(dto, new MediaTypeInfoRelationalDto() { StorageId = idPool[1] }),
            };
            dto.Articles = new List<ArticleRelationalDto>()
            {
                new ArticleRelationalDto() { StorageId = idPool[2] }
            };
            factory.Setup(x => x.ActivateObject(It.Is<Type>(type => type == typeof(ComposerRelationalDto)))).Returns((Type t) => dto);

            var entity = new Composer()
            {
                Profile = new ComposerProfile()
                {
                    Media = new List<MediaTypeInfo>() { new MediaTypeInfo(@"image/*") { StorageId = idPool[0] } }
                }
            };
            
            ComposerRelationalDto result = mapper.BuildDto(entity);

            Assert.AreEqual(1, result.Media.Count);
            Assert.AreEqual(idPool[0], result.Media.First().MediaEntry.StorageId);
        }

        [Test]
        public void PreservesArticleEntries()
        {
            Mock<MockDtoFactory> factory = new Mock<MockDtoFactory>() { CallBase = true };
            factory.SetupAllProperties();
            factory.Object.IntermediateDtoCallback = _defaultIntermediateCallback;
            ComposerTypeMapper mapper = new ComposerTypeMapper(new ComposerMappers(), factory.Object);
            Guid[] idPool = new Guid[] { new Guid(1, 2, 3, new byte[8]), new Guid(4, 5, 6, new byte[8]), new Guid(7, 8, 9, new byte[8]) };
            ComposerRelationalDto dto = new ComposerRelationalDto();
            dto.Media = new List<ComposerMediaRelationalDto>()
            {
                ComposerMediaRelationalDto.Create(dto, new MediaTypeInfoRelationalDto() { StorageId = idPool[0] }),
                ComposerMediaRelationalDto.Create(dto, new MediaTypeInfoRelationalDto() { StorageId = idPool[1] }),
            };
            dto.Articles = new List<ArticleRelationalDto>()
            {
                new ArticleRelationalDto() { StorageId = idPool[2] }
            };
            factory.Setup(x => x.ActivateObject(It.Is<Type>(type => type == typeof(ComposerRelationalDto)))).Returns((Type t) => dto);

            var entity = new Composer()
            {
                Profile = new ComposerProfile()
                {
                    Media = new List<MediaTypeInfo>() { new MediaTypeInfo(@"image/*") { StorageId = idPool[0] } }
                }
            };

            int originalArticlesCount = dto.Articles.Count;

            ComposerRelationalDto result = mapper.BuildDto(entity);

            Assert.AreEqual(originalArticlesCount, result.Articles.Count);

        }
    }

    public class BuildTests : TestFixtureBase
    {
        private readonly ComposerTypeMapper _mapper = new ComposerTypeMapper(new ComposerMappers(), new MockDtoFactory());
        private readonly ComposerRelationalDto _testDto;
        private Composer _entity;

        public BuildTests()
        {
            _testDto = new ComposerRelationalDto()
            {
                ProfilePicture = new MediaTypeInfoRelationalDto()
                {
                    StorageId = new Guid(2, 2, 2, new byte[8]),
                    MimeType = "image/jpeg"
                },
                Articles = new[]
                {
                    new ArticleRelationalDto()
                    {
                        Language = "de-DE",
                        StorageId = new Guid(1, 1, 1, new byte[8]),
                    }
                },
                LocalizedNames = new[] { new NameRelationalDto() { FullName = "John Smith", Language = "de-DE" } },
            };
            _testDto.Media = new List<ComposerMediaRelationalDto>()
            {
                ComposerMediaRelationalDto.Create(_testDto, new MediaTypeInfoRelationalDto()
                {
                    StorageId =  _testDto.ProfilePicture.StorageId,
                    MimeType = _testDto.ProfilePicture.MimeType
                }),
                ComposerMediaRelationalDto.Create(_testDto, new MediaTypeInfoRelationalDto()
                {
                    StorageId = new Guid(3, 3, 3, new byte[8]),
                    MimeType = "image/jpeg"
                })
            };
        }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            _entity = _mapper.Build(_testDto);
        }

        [Test]
        public void MapsArticles()
        {
            Assert.AreEqual(_entity.GetArticle(CultureInfo.GetCultureInfo("de-DE")).StorageId, _testDto.Articles.First().StorageId);
        }

        [Test]
        public void MapsNames()
        {
            Assert.AreEqual(_entity.Name[CultureInfo.GetCultureInfo("de-DE")].FullName, _testDto.LocalizedNames.First().FullName);
        }

        [Test]
        public void MapsMedia()
        {
            IEnumerable<Guid> dtoMediaIds = _testDto.Media.Select(m => m.MediaEntry.StorageId).OrderBy(id => id);
            IEnumerable<Guid> entityMediaIds = _entity.Profile.Media.Select(m => m.StorageId).OrderBy(id => id);

            Assert.IsTrue(entityMediaIds.SequenceEqual(dtoMediaIds));
        }

        [Test]
        public void MapsProfilePicture()
        {
            Assert.IsNotNull(_entity.Profile.ProfilePicture, "Profile picture wasn't copied.");
            Assert.AreEqual(_entity.Profile.ProfilePicture.StorageId, _testDto.ProfilePicture.StorageId);
        }
    }
}
