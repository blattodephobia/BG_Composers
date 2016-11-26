using BGC.Web.Areas.Administration.ViewModels.Permissions;
using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Administration.ViewModels
{
    public class ActivitiesViewModel : ViewModelBase
    {
        private IEnumerable<PermissionViewModelBase> _permittedActions;
        public IEnumerable<PermissionViewModelBase> PermittedActions
        {
            get
            {
                return _permittedActions ?? (_permittedActions = Enumerable.Empty<PermissionViewModelBase>());
            }

            set
            {
                _permittedActions = value;
            }
        }
    }
}