﻿using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Web.Areas.Administration.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TestUtils.MockUtilities;

namespace BGC.Web.Tests.AdministrationArea.Controllers
{
    [TestFixture]
    public class UpdateTests
    {
        private EditController _controller;
        private Composer _composer;
        private Dictionary<Guid, string> _articleStorage;
        private CultureInfo _language;

        [SetUp]
        public void Setup()
        {
            _composer = new Composer();
            _language = CultureInfo.GetCultureInfo("de-DE");
            _composer.LocalizedNames = new List<ComposerName>() { new ComposerName("Petar Stupel", _language) };
            byte[] guid = new byte[16];
            guid[15] = 1;
            _composer.Articles = new List<ComposerArticle>()
            {
                new ComposerArticle(_composer, _language) { StorageId = new Guid(guid) }
            };

            _articleStorage = new Dictionary<Guid, string>()
            {
                { _composer.Articles.First().StorageId, "B" }
            };
            Mock<IArticleContentService> articleService = GetMockArticleService(_articleStorage);
            
            _controller = new EditController(
                composersService: GetMockComposerService(new List<Composer>() { _composer }).Object,
                settingsService: GetMockSettingsService().Object,
                articleStorageService: articleService.Object);
        }

        [Test]
        public void UpdatesMostRecentArticle()
        {
            _controller.Update_Post(new UpdateComposerViewModel()
            {
                ComposerId = _composer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = _composer.GetName(_language).FullName,
                        Content = "b",
                        Language = _language
                    }
                }
            });

            Guid newArticleId = _composer.GetArticle(_language).StorageId;
            Assert.AreEqual(2, _articleStorage.Keys.Count);
            Assert.AreEqual("b", _articleStorage[newArticleId]);
        }

        [Test]
        public void DoesntUpdateArticleIfContentIsSame()
        {
            string sameContent = new string(_articleStorage[_composer.GetArticle(_language).StorageId].ToArray());
            _controller.Update_Post(new UpdateComposerViewModel()
            {
                ComposerId = _composer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = _composer.GetName(_language).FullName,
                        Content = sameContent,
                        Language = _language
                    }
                }
            });

            Guid articleId = _composer.GetArticle(_language).StorageId;
            Assert.AreEqual(sameContent, _articleStorage[articleId]);
            Assert.AreNotSame(sameContent, _articleStorage[articleId]); // strings are equal, but the underlying references haven't been modified
        }

        [Test]
        public void UpdatesName()
        {
            _controller.Update_Post(new UpdateComposerViewModel()
            {
                ComposerId = _composer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = "John Smith",
                        Content = _articleStorage[_composer.GetArticle(_language).StorageId],
                        Language = _language
                    }
                }
            });
            
            Assert.AreEqual("John Smith", _composer.GetName(_language).FullName);
        }

        [Test]
        public void DoesntUpdateNameIfSameFullName()
        {
            string sameName = new string(_composer.GetName(_language).FullName.ToArray());
            _controller.Update_Post(new UpdateComposerViewModel()
            {
                ComposerId = _composer.Id,
                Articles = new List<AddArticleViewModel>()
                {
                    new AddArticleViewModel()
                    {
                        FullName = sameName,
                        Content = _articleStorage[_composer.GetArticle(_language).StorageId],
                        Language = _language
                    }
                }
            });

            Guid articleId = _composer.GetArticle(_language).StorageId;
            Assert.AreEqual(sameName, _composer.GetName(_language).FullName);
            Assert.AreNotSame(sameName, _composer.GetName(_language).FullName); // strings are equal, but the underlying references haven't been modified
        }
    }
}
