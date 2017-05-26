using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public partial class ArticleManagementPermissionViewModel
    {
        public override string LocalizationKey => LocalizationKeys.Administration.Account.Activities.ContentManagement;

        internal override ActionResult ActivityAction => MVC.Administration.Edit.List();
    }
}