using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class ChangePasswordViewModel : PasswordViewModelBase
    {
        [Required]
        public string CurrentPassword { get; set; }
    }
}