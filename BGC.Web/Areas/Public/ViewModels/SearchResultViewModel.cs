using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class SearchResultViewModel : ViewModelBase
    {
        public Guid ResultId { get; set; }

        public string Header { get; set; }

        public ImageViewModel PreviewImage { get; set; }
        
        public string LinkLocation { get; set; }

        public string Content { get; set; }
    }
}