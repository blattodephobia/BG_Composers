using BGC.Web.Areas.Administration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public partial class ApplicationSettingsWritePermissionViewModel : PermissionViewModelBase
    {
        public override string LocalizationKey => LocalizationKeys.Administration.Account.Activities.ApplicationSettings;

        internal override ActionResult ActivityAction => MVC.Administration.Settings.ApplicationSettings();

        public List<SettingWebModel> Settings { get; set; }
    }
}