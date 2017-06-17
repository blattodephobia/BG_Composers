using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class ErrorViewModel : ViewModelBase
    {
        public int StatusCode { get; set; }

        public string Description { get; set; }

        public ErrorViewModel()
        {
        }

        public ErrorViewModel(HttpResponseBase response)
        {
            if (response != null)
            {
                StatusCode = response.StatusCode;
                Description = response.StatusDescription;
            }
        }
    }
}