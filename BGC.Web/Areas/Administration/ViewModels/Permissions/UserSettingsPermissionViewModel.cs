using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public partial class UserSettingsPermissionViewModel : PermissionViewModelBase
    {
        public override string LocalizationKey => LocalizationKeys.Administration.Account.Activities.UserSettings;
    }
}