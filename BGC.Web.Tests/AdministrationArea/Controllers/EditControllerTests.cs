using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestUtils;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Controllers.EditControllerTests
{
    public class UpdateTests : TestFixtureBase
    {
        private readonly EditController _controller;
        private readonly List<Composer> _composerStorage;
        private readonly Dictionary<Guid, string> _articleStorage;
        private readonly Mock<HttpRequestBase> _mockRequest;
        private readonly List<MediaTypeInfo> _mediaStorage;
        private CultureInfo _language;

        private Composer MainTestComposer => _composerStorage[0];

        public UpdateTests()
        {
            _composerStorage = new List<Composer>();
            _articleStorage = new Dictionary<Guid, string>();
            _mediaStorage = new List<MediaTypeInfo>();

            Mock<IArticleContentService> articleService = GetMockArticleService(_articleStorage);

            _controller = new EditController(
                composersService: GetMockComposerService(_composerStorage).Object,
                settingsService: GetMockSettingsService().Object,
                articleStorageService: articleService.Object,
                mediaService: GetMockMediaService(_mediaStorage).Object);
            _mockRequest = GetMockRequestBase(MockBehavior.Loose);
            _mockRequest.Setup(x => x.Url).Returns(new Uri("http://localhost/"));

            _controller.Url = new UrlHelper(GetMockRequestContext(_mockRequest.Object).Object);
            _controller.ControllerContext = new ControllerContext() { HttpContext = GetMockHttpContextBase(_mockRequest.Object).Object };
        }
 
        public override void BeforeEachTest()
        {
            _composerStorage.Clear();
            _articleStorage.Clear();
            _mockRequest.Setup(x => x.Url).Returns(new Uri("http://localhost/"));

            var _composer = new Composer();
            _language = CultureInfo.GetCultureInfo("de-DE");
            _composer.Name[_language] = new ComposerName("Petar Stupel", _language);
            byte[] guid = new byte[16];
            guid[15] = 1;
            _composer.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(_composer, _composer.Name[_language], _language) { StorageId = new Guid(guid) }
            };
            _articleStorage.Add(_composer.Articles.First().StorageId, "B");

            _composerStorage.Add(_composer);
        }

        [Test]
        public void UpdatesMostRecentArticle()
        {
            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = MainTestComposer.Name[_language].FullName,
                        Content = "b",
                        Language = _language
                    }
                }
            });

            Guid newArticleId = MainTestComposer.GetArticle(_language).StorageId;
            Assert.AreEqual(2, _articleStorage.Keys.Count);
            Assert.AreEqual("b", _articleStorage[newArticleId]);
        }

        [Test]
        public void DoesntUpdateArticleIfContentIsSame()
        {
            string sameContent = new string(_articleStorage[MainTestComposer.GetArticle(_language).StorageId].ToArray());
            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = MainTestComposer.Name[_language].FullName,
                        Content = sameContent,
                        Language = _language
                    }
                }
            });

            Guid articleId = MainTestComposer.GetArticle(_language).StorageId;
            Assert.AreEqual(sameContent, _articleStorage[articleId]);
            Assert.AreNotSame(sameContent, _articleStorage[articleId]); // strings are equal, but the underlying references haven't been modified
        }

        [Test]
        public void UpdatesName()
        {
            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = "John Smith",
                        Content = _articleStorage[MainTestComposer.GetArticle(_language).StorageId],
                        Language = _language
                    }
                }
            });
            
            Assert.AreEqual("John Smith", MainTestComposer.Name[_language].FullName);
        }

        [Test]
        public void AssignsCorrectImageLocations()
        {
            Guid localId = new Guid(1, 0, 0, new byte[8]);
            _mediaStorage.Add(new MediaTypeInfo(@"any.jpg", "image/jpeg") { StorageId = localId });
            string externalImageLocation = $"http://google.com/someImage.jpg";

            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Images = new List<ImageViewModel>()
                {
                    new ImageViewModel($"/controller/action?{MVC.Public.Resources.GetParams.resourceId}={localId}"),
                    new ImageViewModel(externalImageLocation)
                }
            });

            IEnumerable<MediaTypeInfo> images = MainTestComposer.Profile.Media;

            Assert.AreEqual(2, images.Count());
            Assert.IsNotNull(images.FirstOrDefault(m => m.ExternalLocation == externalImageLocation), "External image is not present.");
            Assert.IsNotNull(images.FirstOrDefault(m => m.StorageId == localId), "Local image is not present.");
        }

        [Test]
        public void AssignsCorrectImageLocations_DefaultPort()
        {
            _mockRequest.Setup(r => r.Url).Returns(new Uri("https://localhost:443/"));
            Guid localId = new Guid(1, 0, 0, new byte[8]);
            _mediaStorage.Add(new MediaTypeInfo(@"any.jpg", "image/jpeg") { StorageId = localId });

            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Images = new List<ImageViewModel>()
                {
                    new ImageViewModel($"/controller/action?{MVC.Public.Resources.GetParams.resourceId}={localId}")
                }
            });

            IEnumerable<MediaTypeInfo> images = MainTestComposer.Profile.Media;
            Assert.AreEqual(1, images.Count());
            Assert.IsNotNull(images.FirstOrDefault(m => m.StorageId == localId), "Local image is not present.");
        }

        [Test]
        public void AssignsCorrectImageLocations_NonDefaultPort()
        {
            _mockRequest.Setup(r => r.Url).Returns(new Uri("http://localhost:12000/"));
            Guid localId = new Guid(1, 0, 0, new byte[8]);
            _mediaStorage.Add(new MediaTypeInfo(@"any.jpg", "image/jpeg") { StorageId = localId });

            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Images = new List<ImageViewModel>()
                {
                    new ImageViewModel($"/controller/action?{MVC.Public.Resources.GetParams.resourceId}={localId}")
                }
            });

            IEnumerable<MediaTypeInfo> images = MainTestComposer.Profile.Media;
            Assert.AreEqual(1, images.Count());
            Assert.IsNotNull(images.FirstOrDefault(m => m.StorageId == localId), "Local image is not present.");
        }

        [Test]
        public void DoesntUpdateNameIfSameFullName()
        {
            string sameName = new string(MainTestComposer.Name[_language].FullName.ToArray());
            _controller.Update_Post(new AddOrUpdateComposerViewModel()
            {
                ComposerId = MainTestComposer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = sameName,
                        Content = _articleStorage[MainTestComposer.GetArticle(_language).StorageId],
                        Language = _language
                    }
                }
            });

            Assert.AreEqual(sameName, MainTestComposer.Name[_language].FullName);
            Assert.AreNotSame(sameName, MainTestComposer.Name[_language].FullName); // strings are equal, but the underlying references haven't been modified
        }

        [Test]
        public void OverwritesOldMediaCollection()
        {
            MainTestComposer.Profile.Media = new MediaTypeInfo[] { new MediaTypeInfo("image/jpeg") { ExternalLocation = "www.google.com" } };

            string newImageLocation = "http://google.com/else";
            _controller.Update_Post(new AddOrUpdateComposerViewModel(Enumerable.Empty<AddArticleViewModel>(), new[] { new ImageViewModel(newImageLocation) }));

            Assert.AreEqual(1, MainTestComposer.Profile.Media.Count);
            Assert.AreEqual(newImageLocation, MainTestComposer.Profile.Media.First().ExternalLocation);
        }

        [Test]
        public void SetsIsProfilePropertyCorrectly()
        {
            Guid mediaId = new Guid(1, 2, 3, new byte[8]);
            MainTestComposer.Profile.ProfilePicture = new MediaTypeInfo("image/jpeg") { StorageId = mediaId };

            var vm = (_controller.Update(MainTestComposer.Id) as ViewResult).Model as AddOrUpdateComposerViewModel;

            ImageViewModel profilePic = vm.Images.FirstOrDefault(x => x.Location.EndsWith(mediaId.ToString()));
            Assert.IsNotNull(profilePic, "Profile picture ViewModel is not present in page.");
            Assert.IsTrue(profilePic.IsProfilePicture, "Profile picture ViewModel doesn't reflect the image's status.");
        }
    }
}
