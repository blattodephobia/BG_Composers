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
        public string ErrorMessageKey { get; set; }
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}