using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Public.ViewModels;
using BGC.Web.Attributes;
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

        [DefaultAction]
#if !DEBUG
        [OutputCache(Duration = 3600)]
#endif
		public virtual ActionResult Index(char group = '\0')
        {
            group = char.ToUpper(group);
            bool getSpecificGroupOnly = char.IsLetter(group);

            IEnumerable<ComposerArticle> articles = _composersService
                .GetAllComposers()
                .Select(composer => composer.GetArticle(CurrentLocale.EffectiveValue));

            Dictionary<char, IList<ComposerArticle>> articlesIndex = new Dictionary<char, IList<ComposerArticle>>();
            foreach (ComposerArticle article in articles)
            {
                char currentLeadingChar = char.ToUpper(article.LocalizedName.GetEasternOrderFullName()[0]);
                if (!getSpecificGroupOnly || currentLeadingChar == group)
                {
                    if (!articlesIndex.ContainsKey(currentLeadingChar))
                    {
                        articlesIndex.Add(currentLeadingChar, new List<ComposerArticle>());
                    }

                    articlesIndex[currentLeadingChar].Add(article);
                }
            }

            IndexViewModel model = new IndexViewModel()
            {
                Alphabet = getSpecificGroupOnly ? new[] { group }  : LocalizationService.GetAlphabet(useUpperCase: true, culture: CurrentLocale.EffectiveValue),
                Articles = articlesIndex,
            };
            
            return View(model);
        }

        public virtual ActionResult Read(Guid article)
        {
            return View(new ArticleViewModel()
            {
                Text = _articleStorageService.GetEntry(article),
                Title = _composersService.FindComposerByArticle(article)?.GetName(CurrentLocale.EffectiveValue).FullName
            });
        }

        public virtual ActionResult Search(string query)
        {
            var vm = new SearchResultViewModel()
            {
                Results = (from result in _searchService.Search(query)
                           let composer = _composersService.FindComposer(result.IdAsLong())
                           where composer != null
                           select composer.GetArticle(CurrentLocale.EffectiveValue))
                           .ToDictionary(a => a.StorageId, a => a.Composer.GetName(CurrentLocale.EffectiveValue).FullName)
            };
            return View(vm);
        }

        public virtual ActionResult Error()
        {
            return View(new ErrorViewModel(Response));
        }
    }
}
