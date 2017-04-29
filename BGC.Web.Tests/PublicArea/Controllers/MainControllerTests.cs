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
using TestUtils;

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

            foreach (Composer cmp in composers)
            {
                cmp.Articles.Add(new ComposerArticle() { Composer = cmp, LocalizedName = cmp.LocalizedNames.First(), Language = CultureInfo.GetCultureInfo("en-US") });
            }

            return composers;
        }

        [Test]
        public void ReturnsAllComposersIfGroupIsBlank()
        {
            var composers = GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: Mocks.GetMockComposerService(composers).Object,
                articleStorageService: Mocks.GetMockArticleService(composers.SelectMany(c => c.Articles).ToList()).Object,
                composerSearchService: Mocks.GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(Mocks.SampleLocalization);

            var result = mainCtrl.Index() as ViewResult;
            Assert.AreEqual(composers.Count, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());
        }

        [Test]
        public void ReturnsComposersAccordingToGroup()
        {
            var composers = GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: Mocks.GetMockComposerService(composers).Object,
                articleStorageService: Mocks.GetMockArticleService(composers.SelectMany(c => c.Articles).ToList()).Object,
                composerSearchService: Mocks.GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(Mocks.SampleLocalization);

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
                composersService: Mocks.GetMockComposerService(composers).Object,
                articleStorageService: Mocks.GetMockArticleService(composers.SelectMany(c => c.Articles).ToList()).Object,
                composerSearchService: Mocks.GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(Mocks.SampleLocalization);

            var result = mainCtrl.Index() as ViewResult;
            Assert.AreEqual(3, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());

        }

        [Test]
        public void ReturnsAllEntriesWithInvalidCharacter()
        {
            var composers = GetComposers();
            var mainCtrl = new MainControllerProxy(
                composersService: Mocks.GetMockComposerService(composers).Object,
                articleStorageService: Mocks.GetMockArticleService(composers.SelectMany(c => c.Articles).ToList()).Object,
                composerSearchService: Mocks.GetMockComposerSearchService(composers).Object);
            mainCtrl.SetCurrentLocale(CultureInfo.GetCultureInfo("en-US"));
            mainCtrl.LocalizationService = new LocalizationService(Mocks.SampleLocalization);

            var result = mainCtrl.Index('\t') as ViewResult;
            Assert.AreEqual(3, (result.Model as IndexViewModel).Articles.Values.SelectMany(a => a).Count());

        }
    }
}