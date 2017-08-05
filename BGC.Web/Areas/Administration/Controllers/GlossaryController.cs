using BGC.Core;
using BGC.Core.Models;
using BGC.Web.Areas.Administration.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.Controllers
{
    [Permissions(nameof(IGlossaryManagementPermission))]
    public partial class GlossaryController : AdministrationControllerBase
    {
        private readonly IGlossaryService _glossaryService;

        [HttpGet]
        public virtual ActionResult ListGlossary()
        {
            return View(new GlossaryListViewModel(_glossaryService.ListAll().Select(e => GlossaryEntryViewModel.FromEntity(e)).ToList()));
        }

        [HttpGet]
        public virtual ActionResult Edit(Guid? entry)
        {
            if (entry == null)
            {
                return View(new GlossaryEntryViewModel()
                {
                    Definitions = ApplicationProfile.SupportedLanguages.Select(l => new GlossaryDefinitionViewModel() { LocaleCode = l.Name }).ToList()
                });
            }
            else
            {
                GlossaryEntry existingEntry = _glossaryService.Find(entry.Value);
                if (existingEntry != null)
                {
                    return View(GlossaryEntryViewModel.FromEntity(existingEntry));
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName(nameof(Edit))]
        public virtual ActionResult Edit_Post(GlossaryEntryViewModel entry)
        {
            Shield.ArgumentNotNull(entry, nameof(entry)).ThrowOnError();

            var currentEntry = new GlossaryEntry()
            {
                Id = entry.Id.GetValueOrDefault(),
                Definitions = entry.Definitions.Select(d => new GlossaryDefinition(d.GetLocaleCultureInfo(), d.Definition)).ToList()
            };
            _glossaryService.AddOrUpdate(currentEntry);

            return RedirectToAction(MVC.Administration.Glossary.ListGlossary());
        }

        public GlossaryController(IGlossaryService glossaryService)
        {
            Shield.ArgumentNotNull(glossaryService, nameof(glossaryService)).ThrowOnError();

            _glossaryService = glossaryService;
        }
    }
}