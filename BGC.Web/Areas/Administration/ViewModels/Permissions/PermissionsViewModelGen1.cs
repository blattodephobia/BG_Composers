using System.CodeDom.Compiler;
using BGC.Web.Areas.Administration.ViewModels.Permissions;
using BGC.Web.Areas.Administration.Controllers;
using BGC.Utilities;

namespace BGC.Web.Areas.Administration.ViewModels.Permissions
{
    [GeneratedCode("PermissionsViewModelGen.tt", "1.0.0.0")]
    [MappableWith(typeof(BGC.Core.SendInvitePermission))]
    [Discoverable(typeof(AccountController))]
    public partial class SendInvitePermissionViewModel : PermissionViewModelBase
    {
    }
    [GeneratedCode("PermissionsViewModelGen.tt", "1.0.0.0")]
    [MappableWith(typeof(BGC.Core.ApplicationSettingsWritePermission))]
    [Discoverable(typeof(AccountController))]
    public partial class ApplicationSettingsWritePermissionViewModel : PermissionViewModelBase
    {
    }
    [GeneratedCode("PermissionsViewModelGen.tt", "1.0.0.0")]
    [MappableWith(typeof(BGC.Core.UserSettingsPermission))]
    [Discoverable(typeof(AccountController))]
    public partial class UserSettingsPermissionViewModel : PermissionViewModelBase
    {
    }
}
