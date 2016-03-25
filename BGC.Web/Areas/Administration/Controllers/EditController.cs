using BGC.Core;
using BGC.Core.Services;
using BGC.WebAPI.Areas.Administration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
    public partial class EditController : AdministrationControllerBase
    {
        public EditController(IComposerEntriesService composersService)
        {
            this.composersService = composersService;
        }

        public virtual ActionResult List()
        {
            IEnumerable<Composer> composers = this.composersService.GetAllEntries().ToList();
            return this.View(composers);
        }

        [HttpGet]
        public virtual ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [ActionName(nameof(Add))]
        public virtual ActionResult Add_Post(AddComposerViewModel composer)
        {
            this.composersService.Add(new Composer() { Id = 0, LocalizedNames = new List<ComposerName>(), Articles = new List<ComposerEntry>() });
            return this.RedirectToAction(this.List());
        }

        private IComposerEntriesService composersService;
    }
}
