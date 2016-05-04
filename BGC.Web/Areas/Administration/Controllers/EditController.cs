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
        public EditController(IComposerEntriesService composersService)
        {
            this.composersService = composersService.ArgumentNotNull(nameof(composersService)).GetValueOrThrow();
        }

        public virtual ActionResult List()
        {
            IEnumerable<Composer> composers = this.composersService.GetAllEntries().ToList();
            return this.View(composers);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            return this.View(new AddComposerViewModel());
        }

        [HttpPost]
        public virtual ActionResult Add(AddComposerViewModel composer)
        {
            this.composersService.Add(new Composer() { LocalizedNames = new List<ComposerName>() { new ComposerName(composer.Name) }, Articles = new List<ComposerEntry>() });
            return base.RedirectToAction(nameof(List));
        }

        private IComposerEntriesService composersService;
    }
}
