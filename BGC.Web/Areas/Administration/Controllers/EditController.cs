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
        private IComposerEntriesService composersService;
        private ISettingsService settingsService;

        public EditController(IComposerEntriesService composersService, ISettingsService settingsService)
        {
            this.composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
            this.settingsService = settingsService.ArgumentNotNull(nameof(settingsService)).GetValueOrThrow();
        }

        public virtual ActionResult List()
        {
            IEnumerable<Composer> composers = this.composersService.GetAllEntries().ToList();
            return this.View(composers);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            IEnumerable<AddComposerViewModel> articlesRequired =
                settingsService.ReadSetting<CultureSupportSetting>("SupportedLanguages")
                .SupportedCultures
                .Select(c => new AddComposerViewModel() { Language = c });
            return this.View(articlesRequired);
        }

        [HttpPost]
        public virtual ActionResult Add(AddComposerViewModel composer)
        {
            this.composersService.Add(new Composer() { LocalizedNames = new List<ComposerName>() { new ComposerName(composer.FullName) }, Articles = new List<ComposerEntry>() });
            return base.RedirectToAction(nameof(List));
        }
    }
}
