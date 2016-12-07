using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public abstract class PasswordViewModelBase : ViewModelBase
    {
        private const int PASSWORD_MIN_LENGTH = 8;

        [Required(ErrorMessage = LocalizationKeys.Administration.Account.ChangePassword.PasswordRequired)]
        [DataType(DataType.Password)]
        [MinLength(PASSWORD_MIN_LENGTH, ErrorMessage = LocalizationKeys.Administration.Account.ChangePassword.PasswordTooShort)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = LocalizationKeys.Administration.Account.ChangePassword.PasswordsMismatch)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}