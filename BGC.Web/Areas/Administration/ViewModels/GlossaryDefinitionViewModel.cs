using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class GlossaryDefinitionViewModel : ViewModelBase
    {
        public string LocaleCode { get; set; }

        public string Definition { get; set; }
        
        public CultureInfo GetLocaleCultureInfo() => CultureInfo.GetCultureInfo(LocaleCode);
    }
}