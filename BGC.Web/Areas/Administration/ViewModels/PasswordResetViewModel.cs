using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class PasswordResetViewModel : ViewModelBase
    {
        public string Email { get; set; }
        public bool TokenSent { get; set; }
    }
}