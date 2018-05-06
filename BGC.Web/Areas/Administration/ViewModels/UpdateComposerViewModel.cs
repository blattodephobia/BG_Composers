using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class UpdateComposerViewModel : AddComposerViewModel
    {
        public Guid ComposerId { get; set; }

        public int? Order { get; set; }

        public UpdateComposerViewModel()
        {
        }

        public UpdateComposerViewModel(IEnumerable<AddArticleViewModel> articles, IEnumerable<string> imageSources) :
            base(articles, imageSources)
        {
        }
    }
}