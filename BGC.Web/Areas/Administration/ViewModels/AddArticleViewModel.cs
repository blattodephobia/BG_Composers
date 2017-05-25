using BGC.Core;
using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class AddArticleViewModel : ViewModelBase
    {
        public string FullName { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public CultureInfo Language { get; set; }
    }
}