using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.ViewModels
{
    public abstract partial class ViewModelBase
    {
        protected ViewModelBase()
        {
            ErrorMessages = new string[0];
        }

        public string[] ErrorMessages { get; set; }
    }
}