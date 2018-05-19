using BGC.Core;
using BGC.Core.Services;
using BGC.Services;
using BGC.Utilities;
using BGC.Web.Areas.Public.Controllers;
using BGC.Web.Areas.Public.ViewModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.PublicArea.Controllers
{
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
        private static List<Composer> GetComposers()
        {
            var composers = new List<Composer>()
            {
                new Composer()
                {
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Smith", "en-US") },
                    Articles = new List<ComposerArticle>()
                },
                new Composer()
                {
                    LocalizedNames = new List<ComposerName>() { new ComposerName("Peter Samuel", "en-US") },
                    Articles = new List<ComposerArticle>()
                },
                new Composer()
                {
                    LocalizedNames = new List<ComposerName>() { new ComposerName("John Nappa", "en-US") },
                    Articles = new List<ComposerArticle>()
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

        [Test]
        public void ReturnsAllComposersIfGroupIsBlank()
        {
            var composers = GetComposers();
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
            var composers = GetComposers();
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
            var composers = GetComposers();
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
            var composers = GetComposers();
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
}