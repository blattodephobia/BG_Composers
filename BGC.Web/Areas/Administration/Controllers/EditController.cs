using BGC.Core;
using BGC.Core.Services;
using BGC.Web.Areas.Administration.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    public partial class EditController : AdministrationControllerBase
    {
        private IComposerDataService composersService;
        private ISettingsService settingsService;
        private IDataStorageService articleStorageService;

        public EditController(IComposerDataService composersService, ISettingsService settingsService, IDataStorageService articleStorageService)
        {
            this.composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
            this.articleStorageService = articleStorageService.ArgumentNotNull(nameof(articleStorageService)).GetValueOrThrow();
        }

        public virtual ActionResult List()
        {
            IEnumerable<Composer> composers = this.composersService.GetAllComposers().ToList();
            return this.View(composers);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            IList<AddComposerViewModel> articlesRequired =
                settingsService.ReadSetting<CultureSupportSetting>("SupportedLanguages")
                .SupportedCultures
                .Select(c => new AddComposerViewModel() { Language = c })
                .ToList();

            return this.View(articlesRequired);
        }

        [HttpPost]
        public virtual ActionResult Add(IList<AddComposerViewModel> editedData)
        {
            Composer newComposer = new Composer();
            for (int i = 0; i < editedData.Count; i++)
            {
                newComposer.LocalizedNames.Add(new ComposerName(editedData[i].FullName));
                newComposer.Articles.Add(new ComposerArticle()
                {
                    StorageId = this.articleStorageService.StoreEntry(editedData[i].Article),
                    Composer = newComposer,
                    Language = editedData[i].Language
                });
            }
            this.composersService.Add(newComposer);

            return base.RedirectToAction(nameof(List));
        }
    }
}
