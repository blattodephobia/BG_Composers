using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Public.ViewModels;
using BGC.Web.Controllers;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public.Controllers
{
	public partial class MainController : BgcControllerBase
    {
        private readonly IComposerDataService _composersService;
        private readonly IArticleContentService _articleStorageService;
        private readonly ISearchService _searchService;

        public MainController(IComposerDataService composersService, IArticleContentService articleStorageService, [Dependency(nameof(Composer))] ISearchService composerSearchService)
        {
			_composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            _articleStorageService = articleStorageService.ArgumentNotNull(nameof(_articleStorageService)).GetValueOrThrow();
            _searchService = composerSearchService.ArgumentNotNull(nameof(composerSearchService)).GetValueOrThrow();
        }

		public virtual ActionResult Index()
        {
            IEnumerable<ComposerArticle> articles = _composersService
                .GetAllComposers()
                .Select(composer => composer.GetArticle(CurrentLocale));
            return View(articles.GroupBy(article => article.LocalizedName.LastName[0]));
        }

        public virtual ActionResult Read(Guid article)
        {
            return View(new ArticleViewModel() { Text = this._articleStorageService.GetEntry(article) });
        }

        public virtual ActionResult Search(string query)
        {
            var vm = new SearchResultViewModel()
            {
                Results = (from result in _searchService.Search(query)
                           let composer = _composersService.FindComposer(result.IdAsLong())
                           where composer != null
                           select composer.GetArticle(CurrentLocale))
                           .ToDictionary(a => a.StorageId, a => a.Composer.GetName(CurrentLocale).FullName)
            };
            return View(vm);
        }
    }
}
