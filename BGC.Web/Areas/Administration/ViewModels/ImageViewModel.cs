using BGC.Web.ViewModels;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class ImageViewModel : ViewModelBase
    {
        public string Location { get; set; }

        public bool Preferred { get; set; }

        public ImageViewModel()
        {
            Location = string.Empty;
        }

        public ImageViewModel(string location)
        {
            Shield.IsNotNullOrEmpty(location).ThrowOnError();

            Location = location;
        }
    }
}