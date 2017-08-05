using BGC.Web.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class GlossaryListViewModel : ViewModelBase
    {
        public IEnumerable<GlossaryEntryViewModel> Entries { get; set; }

        public GlossaryListViewModel() :
            this(Enumerable.Empty<GlossaryEntryViewModel>())
        {
        }

        public GlossaryListViewModel(IEnumerable<GlossaryEntryViewModel> entries)
        {
            Shield.ArgumentNotNull(entries, nameof(entries)).ThrowOnError();

            Entries = entries.ToList();
        }
    }
}