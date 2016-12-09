using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class RequestPasswordResetViewModel : ViewModelBase
    {
        [Required] public string Email { get; set; }

        public bool IsEmailSent { get; set; }
    }
}