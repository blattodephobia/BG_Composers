using BGC.Web.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class AddComposerViewModel : ViewModelBase
    {
        public List<AddArticleViewModel> Articles { get; set; }
        
        public List<ImageViewModel> Images { get; set; }

        public AddComposerViewModel()
        {
            Articles = new List<AddArticleViewModel>();
            Images = new List<ImageViewModel>();
        }

        public AddComposerViewModel(IEnumerable<AddArticleViewModel> articles)
        {
            Shield.ArgumentNotNull(articles).ThrowOnError();

            Articles = articles.ToList();
            Images = new List<ImageViewModel>();
        }

        public AddComposerViewModel(IEnumerable<AddArticleViewModel> articles, IEnumerable<ImageViewModel> imageSources)
        {
            Shield.ArgumentNotNull(articles).ThrowOnError();
            Shield.ArgumentNotNull(imageSources).ThrowOnError();

            Articles = articles.ToList();
            Images = imageSources.ToList();
        }
    }
}