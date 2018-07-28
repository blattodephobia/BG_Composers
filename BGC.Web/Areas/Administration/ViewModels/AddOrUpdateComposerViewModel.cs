using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class AddOrUpdateComposerViewModel : ViewModelBase
    {
        public List<AddArticleViewModel> Articles { get; set; }

        public List<ImageViewModel> Images { get; set; }

        public Guid ComposerId { get; set; }

        public int? Order { get; set; }

        public AddOrUpdateComposerViewModel()
            : this(null, null)
        {
        }

        public AddOrUpdateComposerViewModel(IEnumerable<AddArticleViewModel> articles, IEnumerable<ImageViewModel> imageSources = null)
        {
            Articles = articles?.ToList() ?? new List<AddArticleViewModel>();
            Images = imageSources?.ToList() ?? new List<ImageViewModel>();
        }
    }
}