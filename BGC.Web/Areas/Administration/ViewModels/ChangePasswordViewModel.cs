using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class ChangePasswordViewModel : ViewModelBase
    {
        private const int PASSWORD_MIN_LENGTH = 8;

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(PASSWORD_MIN_LENGTH)]
        public string NewPassword { get; set; }
        
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}