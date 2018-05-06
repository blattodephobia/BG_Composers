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
        
        public List<string> ImageSources { get; set; }

        public AddComposerViewModel()
        {
            Articles = new List<AddArticleViewModel>();
            ImageSources = new List<string>();
        }

        public AddComposerViewModel(IEnumerable<AddArticleViewModel> articles)
        {
            Shield.ArgumentNotNull(articles).ThrowOnError();

            Articles = articles.ToList();
            ImageSources = new List<string>();
        }

        public AddComposerViewModel(IEnumerable<AddArticleViewModel> articles, IEnumerable<string> imageSources)
        {
            Shield.ArgumentNotNull(articles).ThrowOnError();
            Shield.ArgumentNotNull(imageSources).ThrowOnError();

            Articles = articles.ToList();
            ImageSources = imageSources.ToList();
        }
    }
}