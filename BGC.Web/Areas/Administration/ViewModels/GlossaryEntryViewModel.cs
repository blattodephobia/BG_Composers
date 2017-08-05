using BGC.Core.Models;
using BGC.Web.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class GlossaryEntryViewModel : ViewModelBase
    {
        public List<GlossaryDefinitionViewModel> Definitions { get; set; }

        public Guid? Id { get; set; }

        public GlossaryEntryViewModel()
        {
            Definitions = new List<GlossaryDefinitionViewModel>();
        }

        public GlossaryEntryViewModel(int emptyDefinitionsCount)
        {
            Shield.AssertOperation(emptyDefinitionsCount, emptyDefinitionsCount > 0, $"{nameof(emptyDefinitionsCount)} must be greater than zero.").ThrowOnError();

            Definitions = Enumerable.Range(0, emptyDefinitionsCount).Select(i => new GlossaryDefinitionViewModel()).ToList();
        }

        public IEnumerable<GlossaryDefinitionViewModel> GetDefinitionsInLocale(params CultureInfo[] locales) =>
            GetDefinitionsInLocale(new HashSet<CultureInfo>(locales));

        public IEnumerable<GlossaryDefinitionViewModel> GetDefinitionsInLocale(HashSet<CultureInfo> locales)
        {
            return Definitions?.Where(def => locales.Contains(def.GetLocaleCultureInfo()));
        }

        public static GlossaryEntryViewModel FromEntity(GlossaryEntry entry)
        {
            Shield.ArgumentNotNull(entry, nameof(entry)).ThrowOnError();

            var definitions = entry.Definitions.Select(def => new GlossaryDefinitionViewModel()
            {
                LocaleCode = def.Language.Name,
                Definition = def.Definition,
                Term = def.Term
            });
            var result = new GlossaryEntryViewModel()
            {
                Definitions = definitions.ToList(),
                Id = entry.Id,
            };

            return result;
        }
    }
}