using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Public.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Public.Controllers
{
	public partial class MainController : Controller
    {
        private IComposerDataService composersService;
        private IDataStorageService articlesStorageService;

		public MainController(IComposerDataService composersService, IDataStorageService articlesStorageService)
		{
			this.composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.articlesStorageService = articlesStorageService.ArgumentNotNull(nameof(articlesStorageService)).GetValueOrThrow();
		}

		public virtual ActionResult Index()
        {
            IEnumerable<ComposerArticle> articles = this.composersService
                .GetAllComposers()
                .Select(composer => composer.GetArticle(CultureInfo.GetCultureInfo("bg-BG")));
            return View(articles.GroupBy(article => article.LocalizedName.LastName[0]));
        }

        public virtual ActionResult Read(Guid article)
        {
            return View(new ArticleViewModel() { Text = this.articlesStorageService.GetEntry(article) });
        }
    }
}
