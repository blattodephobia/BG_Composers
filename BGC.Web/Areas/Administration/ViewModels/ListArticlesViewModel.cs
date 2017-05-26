using BGC.Core;
using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class ListArticlesViewModel : ViewModelBase
    {
        public List<Composer> Composers { get; set; }

        public ListArticlesViewModel()
        {
            Composers = new List<Composer>();
        }

        public ListArticlesViewModel(IEnumerable<Composer> composers)
        {
            Composers = composers?.ToList() ?? new List<Composer>();
        }
    }
}