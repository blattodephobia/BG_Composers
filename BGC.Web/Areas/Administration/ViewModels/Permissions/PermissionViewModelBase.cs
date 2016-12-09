using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    public abstract class PermissionViewModelBase : ViewModelBase
    {
        public abstract string LocalizationKey { get; }

        /// <summary>
        /// The value of this property should be set by the controller using this view model from the specified <see cref="ActivityAction"/>.
        /// </summary>
        public string ActivityUrl { get; set; }

        internal virtual ActionResult ActivityAction => null;
    }
}