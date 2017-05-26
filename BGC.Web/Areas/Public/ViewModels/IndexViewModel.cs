using BGC.Core;
using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class IndexViewModel : ViewModelBase
    {
        public char[] Alphabet { get; set; }

        public Dictionary<char, IList<ComposerArticle>> Articles { get; set; }
    }
}