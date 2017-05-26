using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class AddComposerViewModel : ViewModelBase
    {
        public List<AddArticleViewModel> Articles { get; set; }

        public AddComposerViewModel()
        {
            Articles = new List<AddArticleViewModel>();
        }

        public AddComposerViewModel(IEnumerable<AddArticleViewModel> articles)
        {
            Articles = articles.ToList();
        }
    }
}