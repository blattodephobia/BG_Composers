using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class SearchResultsViewModel : ViewModelBase
    {
        public List<SearchResultViewModel> Results { get; set; } = new List<SearchResultViewModel>();
    }
}