using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Public.ViewModels;
using BGC.Web.Attributes;
using BGC.Web.Controllers;
using BGC.Web.ViewModels;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static BGC.Web.WebApiApplication;

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

            IEnumerable<Composer> composers = _composersService
                .GetAllComposers()
                .Where(composer => composer.FindArticle(CurrentLocale.EffectiveValue) != null);

            Dictionary<char, IList<ComposerArticle>> articlesIndex = new Dictionary<char, IList<ComposerArticle>>();
            foreach (Composer composer in composers)
            {
                char currentLeadingChar = char.ToUpper(composer.Name[CurrentLocale.EffectiveValue].GetEasternOrderFullName()[0]);
                if (!getSpecificGroupOnly || currentLeadingChar == group)
                {
                    if (!articlesIndex.ContainsKey(currentLeadingChar))
                    {
                        articlesIndex.Add(currentLeadingChar, new List<ComposerArticle>());
                    }

                    articlesIndex[currentLeadingChar].Add(composer.GetArticle(CurrentLocale.EffectiveValue));
                }
            }

            IndexViewModel model = new IndexViewModel()
            {
                Alphabet = getSpecificGroupOnly
                    ? new[] { group }
                    : LocalizationService.GetAlphabet(useUpperCase: true, culture: CurrentLocale.EffectiveValue),
                Articles = articlesIndex,
            };
            
            return View(model);
        }

        public virtual ActionResult Read(Guid composerId)
        {
            Composer composer = _composersService.FindComposer(composerId);
            var vm = new ArticleViewModel()
            {
                Title = composer.Name[CurrentLocale.EffectiveValue].FullName,
                Text = _articleStorageService.GetEntry(composer.GetArticle(CurrentLocale.EffectiveValue).StorageId),
            };

            MediaTypeInfo profilePic = composer.Profile.ProfilePicture;
            if (profilePic != null)
            {
                bool isExternal = profilePic.ExternalLocation != null;
                string location = isExternal
                    ? profilePic.ExternalLocation
                    : ResourcesController.GetLocalResourceUri(Url, profilePic.StorageId).PathAndQuery;
                vm.ProfilePicture = new ImageViewModel(location);
            }

            return View(vm);
        }

        public virtual ActionResult Search(string query)
        {
            var vm = new SearchResultsViewModel();
            vm.Results = (from result in _searchService.Search(query, CurrentLocale.EffectiveValue).OfType<ComposerSearchResult>()
                          let preview = result.Preview
                          select new SearchResultViewModel()
                          {
                              ResultId = result.IdAsGuid(),
                              Header = result.Header,
                              Content = _articleStorageService.GeneratePreview(result.ArticlePreview.StorageId)?.OuterXml ?? "",
                              LinkLocation = Url.Action(MVC.Public.Main.Read().AddRouteValue(MVC.Public.Main.ReadParams.composerId, result.IdAsGuid())),
                              PreviewImage = preview != null
                                 ? new ImageViewModel(ResourcesController.GetLocalResourceUri(Url, preview.StorageId).PathAndQuery)
                                 : null
                          }).ToList();
            return View(vm);
        }

        public virtual ActionResult Error()
        {
            ErrorViewModel vm = HttpContext.Session[DataKeys.Global.ErrorViewModelKey] as ErrorViewModel ?? new ErrorViewModel(Response);
            return View(vm);
        }
    }
}
