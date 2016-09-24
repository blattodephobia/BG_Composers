using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public partial class SendInvitePermissionViewModel : PermissionViewModelBase
    {
        public override string LocalizationKey => LocalizationKeys.Administration.Account.Activities.SendInvite;

        public string Email { get; set; }
    }
}