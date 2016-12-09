using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public partial class SendInvitePermissionViewModel : PermissionViewModelBase
    {
        public override string LocalizationKey => LocalizationKeys.Administration.Account.Activities.SendInvite;

        public string Email { get; set; }

        internal override ActionResult ActivityAction => MVC.AdministrationArea.UserManagement.SendInvite();

        public List<string> AvailableRoles { get; set; }

        public bool IsPreviousInvitationSent { get; set; }
    }
}