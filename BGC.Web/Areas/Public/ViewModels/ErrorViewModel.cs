using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public ErrorViewModel(int statusCode, string description)
        {
            StatusCode = statusCode;
            Description = description;
        }

        public ErrorViewModel(HttpStatusCode statusCode, string description) :
            this((int)statusCode, description)
        {
        }

        public ErrorViewModel(HttpResponseBase response) :
            this(response?.StatusCode ?? 0, response?.StatusDescription)
        {
        }

        public ErrorViewModel(HttpResponse response) :
            this(response?.StatusCode ?? 0, response?.StatusDescription)
        {
        }
    }
}