using BGC.Core;
using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class AddComposerViewModel : ViewModelBase
    {
        public string FullName { get; set; }

        public string Article { get; set; }

        public CultureInfo Language { get; set; }
    }
}