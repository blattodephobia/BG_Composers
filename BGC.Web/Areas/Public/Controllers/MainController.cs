using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Public.ViewModels;
using BGC.Web.Controllers;
using CodeShield;
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
        private IComposerDataService _composersService;
        private IArticleContentService articlesStorageService;

		public MainController(IComposerDataService composersService, IArticleContentService articlesStorageService)
		{
			_composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.articlesStorageService = articlesStorageService.ArgumentNotNull(nameof(articlesStorageService)).GetValueOrThrow();
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
            return View(new ArticleViewModel() { Text = this.articlesStorageService.GetEntry(article) });
        }

        public virtual ActionResult Search(string query)
        {
            string[] keywords = query.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var foundEntities = from composer in _composersService.GetAllComposers()
                                from name in composer.LocalizedNames
                                from keyword in keywords
                                where name.Language == CurrentLocale && name.FullName.Contains(keyword)
                                select composer;
            SearchResultViewModel vm = new SearchResultViewModel()
            {
                Results = foundEntities.ToDictionary(c => c.GetArticle(CurrentLocale).StorageId, c => c.GetName(CurrentLocale).GetEasternOrderFullName())
            };
            return View(vm);
        }
    }
}
