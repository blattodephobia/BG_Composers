using BGC.Core;
using BGC.Core.Services;
using BGC.Services;
using BGC.Utilities;
using BGC.Web.Areas.Public.Controllers;
using BGC.Web.Areas.Public.ViewModels;
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
using static TestUtils.MvcUtilities;

namespace BGC.Web.PublicArea.Controllers.MainControllerTests
{
    public class TestData
    {
        public static List<Composer> GetComposers()
        {
            var composers = new List<Composer>()
            {
                new Composer()
                {
                    Id = new Guid(1, 0, 0, new byte[8]),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") },
                    Articles = new List<ComposerArticle>(),
                    Profile = new ComposerProfile() { ProfilePicture = new MediaTypeInfo(@"image/jpeg") { StorageId = new Guid(1, 0, 0, new byte[8]) } }
                },
                new Composer()
                {
                    Id = new Guid(2, 0, 0, new byte[8]),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("Peter Samuel", "en-US") },
                    Articles = new List<ComposerArticle>(),
                    Profile = new ComposerProfile() { ProfilePicture = new MediaTypeInfo(@"image/jpeg") { StorageId = new Guid(2, 0, 0, new byte[8]) } }
                },
                new Composer()
                {
                    Id = new Guid(3, 0, 0, new byte[8]),
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Nappa", "en-US") },
                    Articles = new List<ComposerArticle>(),
                    Profile = new ComposerProfile() { ProfilePicture = new MediaTypeInfo(@"image/jpeg") { StorageId = new Guid(3, 0, 0, new byte[8]) } }
                },
            };

            for (int i = 0; i < composers.Count; i++)
            {
                Composer cmp = composers[i];
                cmp.AddArticle(new ComposerArticle(cmp, cmp.Name[CultureInfo.GetCultureInfo("en-US")], CultureInfo.GetCultureInfo("en-US"))
                {
                    LocalizedName = cmp.LocalizedNames.First(),
                    StorageId = new Guid(Enumerable.Range(0, 16).Select((_byte, index) => index == 15 ? (byte)(i) : (byte)0).ToArray())
                });
            }

            return composers;
        }
    }
    public class MainControllerProxy : MainController
    {
        private class DummyDependencyValue : DependencyValue<CultureInfo>
        {
            public DummyDependencyValue(CultureInfo value) :
                base(value)
            {
            }
        }

        public MainControllerProxy(IComposerDataService composersService, IArticleContentService articleStorageService, ISearchService composerSearchService) :
            base(composersService, articleStorageService, composerSearchService)
        {
            Mock<HttpRequestBase> mockReqBase = GetMockRequestBase(MockBehavior.Loose);
            mockReqBase.Setup(x => x.Url).Returns(new Uri("http://localhost/"));
            Url = new UrlHelper(GetMockRequestContext(mockReqBase.Object).Object);
        }

        public void SetCurrentLocale(DependencyValue<CultureInfo> value)
        {
            CurrentLocale = value;
        }

        public void SetCurrentLocale(CultureInfo value)
        {
            CurrentLocale = new DummyDependencyValue(value);
        }
    }

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void ReturnsAllComposersIfGroupIsBlank()
        {
            var composers = TestData.GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: GetMockComposerService(composers).Object,
                articleStorageService: GetMockArticleService(composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => "")).Object,
                composerSearchService: GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(SampleLocalization);

            var result = mainCtrl.Index() as ViewResult;
            Assert.AreEqual(composers.Count, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());
        }

        [Test]
        public void ReturnsComposersAccordingToGroup()
        {
            var composers = TestData.GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: GetMockComposerService(composers).Object,
                articleStorageService: GetMockArticleService(composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => "")).Object,
                composerSearchService: GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(SampleLocalization);

            var resultUpperCase = mainCtrl.Index('S') as ViewResult;
            Assert.AreEqual(2, (resultUpperCase.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());

            var resultLowerCase = mainCtrl.Index('s') as ViewResult;
            Assert.AreEqual(2, (resultLowerCase.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());
        }

        [Test]
        public void ReturnsAllEntriesWithBlankParameter()
        {
            var composers = TestData.GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: GetMockComposerService(composers).Object,
                articleStorageService: GetMockArticleService(composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => "")).Object,
                composerSearchService: GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(SampleLocalization);

            var result = mainCtrl.Index() as ViewResult;
            Assert.AreEqual(3, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());
        }

        [Test]
        public void ReturnsAllEntriesWithInvalidCharacter()
        {
            var composers = TestData.GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: GetMockComposerService(composers).Object,
                articleStorageService: GetMockArticleService(composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => "")).Object,
                composerSearchService: GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(SampleLocalization);

            var result = mainCtrl.Index('\t') as ViewResult;
            Assert.AreEqual(3, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());
        }
    }

    public class SearchTests : TestFixtureBase
    {
        private readonly MainControllerProxy _ctrl;
        private readonly Mock<IComposerDataService> _composerService;
        private readonly Mock<IArticleContentService> _articleService;
        private readonly Mock<ISearchService> _searchService;

        public SearchTests()
        {
            var composers = TestData.GetComposers();
            _composerService = GetMockComposerService(composers);
            _articleService = GetMockArticleService(composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => ""));
            _searchService = GetMockComposerSearchService(composers);

            _ctrl = new MainControllerProxy(
                composersService: _composerService.Object,
                articleStorageService: _articleService.Object,
                composerSearchService: _searchService.Object);
            _ctrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            _ctrl.LocalizationService = new LocalizationService(SampleLocalization);
        }

        [Test]
        public void GeneratesPreview()
        {
            var vm = ExtractViewModel<SearchResultsViewModel>(_ctrl.Search(@"Peter"));

            Assert.IsNotNull(vm.Results[0].PreviewImage);
        }

        [Test]
        public void SetsStringEmptyIfNullArticlePreview()
        {
            var vm = ExtractViewModel<SearchResultsViewModel>(_ctrl.Search(@"Peter"));

            Assert.AreEqual(string.Empty, vm.Results[0].Content);
        }
    }

    public class ReadTests : TestFixtureBase
    {
        private readonly MainControllerProxy _ctrl;
        private readonly Mock<IComposerDataService> _composerService;
        private readonly Mock<IArticleContentService> _articleService;
        private readonly Mock<ISearchService> _searchService;
        private readonly List<Composer> _composers;

        public ReadTests()
        {
            _composers = TestData.GetComposers();
            _composers.First(c => c.Name[new CultureInfo("en-US")].LastName == "Smith").Profile.ProfilePicture = null;

            _composerService = GetMockComposerService(_composers);
            _articleService = GetMockArticleService(_composers.SelectMany(c => c.Articles).ToDictionary(a => a.StorageId, a => ""));
            _searchService = GetMockComposerSearchService(_composers);

            _ctrl = new MainControllerProxy(
                composersService: _composerService.Object,
                articleStorageService: _articleService.Object,
                composerSearchService: _searchService.Object);
            _ctrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            _ctrl.LocalizationService = new LocalizationService(SampleLocalization);
        }

        [Test]
        public void MapsProfilePictureIfNotNull()
        {
            var vm = ExtractViewModel<ArticleViewModel>(_ctrl.Read(_composers[1].Id));

            Assert.IsNotNull(vm.ProfilePicture);
        }

        [Test]
        public void DoesntMapProfilePictureIfNull()
        {
            var vm = ExtractViewModel<ArticleViewModel>(_ctrl.Read(_composers[0].Id));

            Assert.IsNull(vm.ProfilePicture);
        }
    }
}