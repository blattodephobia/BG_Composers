using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class RegisterViewModel : PasswordViewModelBase
    {
        public Guid InvitationId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.Administration.UserManagement.Register.UserNameRequired)]
        public string UserName { get; set; }
    }
}