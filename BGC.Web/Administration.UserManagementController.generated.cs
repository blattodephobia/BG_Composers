// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo. Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace BGC.Web.Areas.Administration.Controllers
{
    public partial class UserManagementController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected UserManagementController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult SendInvite_Post()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SendInvite_Post);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Register()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Register_Post()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register_Post);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ChangePassword_Post()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangePassword_Post);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ResetPassword()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ResetPassword_Post()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword_Post);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> RequestPasswordReset_Post()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RequestPasswordReset_Post);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }
        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.ActionResult Login_Post()
        {
            return new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login_Post);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public UserManagementController Actions { get { return MVC.AdministrationArea.UserManagement; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Administration";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "UserManagement";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "UserManagement";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string SendInvite = "SendInvite";
            public readonly string SendInvite_Post = nameof(SendInvite);
            public readonly string Register = "Register";
            public readonly string Register_Post = nameof(Register);
            public readonly string Activities = "Activities";
            public readonly string ChangePassword = "ChangePassword";
            public readonly string ChangePassword_Post = nameof(ChangePassword);
            public readonly string ResetPassword = "ResetPassword";
            public readonly string ResetPassword_Post = nameof(ResetPassword);
            public readonly string RequestPasswordReset = "RequestPasswordReset";
            public readonly string RequestPasswordReset_Post = nameof(RequestPasswordReset);
            public readonly string Login = "Login";
            public readonly string Login_Post = nameof(Login);
            public readonly string LogOut = "LogOut";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string SendInvite = "SendInvite";
            public const string SendInvite_Post = nameof(SendInvite);
            public const string Register = "Register";
            public const string Register_Post = nameof(Register);
            public const string Activities = "Activities";
            public const string ChangePassword = "ChangePassword";
            public const string ChangePassword_Post = nameof(ChangePassword);
            public const string ResetPassword = "ResetPassword";
            public const string ResetPassword_Post = nameof(ResetPassword);
            public const string RequestPasswordReset = "RequestPasswordReset";
            public const string RequestPasswordReset_Post = nameof(RequestPasswordReset);
            public const string Login = "Login";
            public const string Login_Post = nameof(Login);
            public const string LogOut = "LogOut";
        }


        static readonly ActionParamsClass_SendInvite s_params_SendInvite = new ActionParamsClass_SendInvite();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SendInvite SendInviteParams { get { return s_params_SendInvite; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SendInvite
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_SendInvite_Post s_params_SendInvite_Post = new ActionParamsClass_SendInvite_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_SendInvite_Post SendInvite_PostParams { get { return s_params_SendInvite_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_SendInvite_Post
        {
            public readonly string invitation = "invitation";
        }
        static readonly ActionParamsClass_Register s_params_Register = new ActionParamsClass_Register();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Register RegisterParams { get { return s_params_Register; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Register
        {
            public readonly string invitation = "invitation";
        }
        static readonly ActionParamsClass_Register_Post s_params_Register_Post = new ActionParamsClass_Register_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Register_Post Register_PostParams { get { return s_params_Register_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Register_Post
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_ChangePassword s_params_ChangePassword = new ActionParamsClass_ChangePassword();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChangePassword ChangePasswordParams { get { return s_params_ChangePassword; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChangePassword
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_ChangePassword_Post s_params_ChangePassword_Post = new ActionParamsClass_ChangePassword_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ChangePassword_Post ChangePassword_PostParams { get { return s_params_ChangePassword_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ChangePassword_Post
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_ResetPassword s_params_ResetPassword = new ActionParamsClass_ResetPassword();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ResetPassword ResetPasswordParams { get { return s_params_ResetPassword; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ResetPassword
        {
            public readonly string referrer = "referrer";
            public readonly string token = "token";
        }
        static readonly ActionParamsClass_ResetPassword_Post s_params_ResetPassword_Post = new ActionParamsClass_ResetPassword_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_ResetPassword_Post ResetPassword_PostParams { get { return s_params_ResetPassword_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_ResetPassword_Post
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_RequestPasswordReset s_params_RequestPasswordReset = new ActionParamsClass_RequestPasswordReset();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RequestPasswordReset RequestPasswordResetParams { get { return s_params_RequestPasswordReset; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RequestPasswordReset
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_RequestPasswordReset_Post s_params_RequestPasswordReset_Post = new ActionParamsClass_RequestPasswordReset_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RequestPasswordReset_Post RequestPasswordReset_PostParams { get { return s_params_RequestPasswordReset_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RequestPasswordReset_Post
        {
            public readonly string vm = "vm";
        }
        static readonly ActionParamsClass_Login s_params_Login = new ActionParamsClass_Login();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Login LoginParams { get { return s_params_Login; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Login
        {
            public readonly string model = "model";
        }
        static readonly ActionParamsClass_Login_Post s_params_Login_Post = new ActionParamsClass_Login_Post();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_Login_Post Login_PostParams { get { return s_params_Login_Post; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_Login_Post
        {
            public readonly string model = "model";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Register = "Register";
                public readonly string SendInvite = "SendInvite";
            }
            public readonly string Register = "~/Areas/Administration/Views/UserManagement/Register.cshtml";
            public readonly string SendInvite = "~/Areas/Administration/Views/UserManagement/SendInvite.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_UserManagementController : BGC.Web.Areas.Administration.Controllers.UserManagementController
    {
        public T4MVC_UserManagementController() : base(Dummy.Instance) { }

        [NonAction]
        partial void SendInviteOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.Permissions.SendInvitePermissionViewModel vm);

        [NonAction]
        public override System.Web.Mvc.ActionResult SendInvite(BGC.Web.Areas.Administration.ViewModels.Permissions.SendInvitePermissionViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SendInvite);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            SendInviteOverride(callInfo, vm);
            return callInfo;
        }

        [NonAction]
        partial void SendInvite_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.Permissions.SendInvitePermissionViewModel invitation);

        [NonAction]
        public override System.Web.Mvc.ActionResult SendInvite_Post(BGC.Web.Areas.Administration.ViewModels.Permissions.SendInvitePermissionViewModel invitation)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.SendInvite_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "invitation", invitation);
            SendInvite_PostOverride(callInfo, invitation);
            return callInfo;
        }

        [NonAction]
        partial void RegisterOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, System.Guid invitation);

        [NonAction]
        public override System.Web.Mvc.ActionResult Register(System.Guid invitation)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "invitation", invitation);
            RegisterOverride(callInfo, invitation);
            return callInfo;
        }

        [NonAction]
        partial void Register_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.RegisterViewModel vm);

        [NonAction]
        public override System.Web.Mvc.ActionResult Register_Post(BGC.Web.Areas.Administration.ViewModels.RegisterViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Register_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            Register_PostOverride(callInfo, vm);
            return callInfo;
        }

        [NonAction]
        partial void ActivitiesOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Activities()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Activities);
            ActivitiesOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void ChangePasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.ChangePasswordViewModel vm);

        [NonAction]
        public override System.Web.Mvc.ActionResult ChangePassword(BGC.Web.Areas.Administration.ViewModels.ChangePasswordViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangePassword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            ChangePasswordOverride(callInfo, vm);
            return callInfo;
        }

        [NonAction]
        partial void ChangePassword_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.ChangePasswordViewModel vm);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ChangePassword_Post(BGC.Web.Areas.Administration.ViewModels.ChangePasswordViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ChangePassword_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            ChangePassword_PostOverride(callInfo, vm);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        [NonAction]
        partial void ResetPasswordOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, string referrer, string token);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ResetPassword(string referrer, string token)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "referrer", referrer);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "token", token);
            ResetPasswordOverride(callInfo, referrer, token);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        [NonAction]
        partial void ResetPassword_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.PasswordResetViewModel vm);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> ResetPassword_Post(BGC.Web.Areas.Administration.ViewModels.PasswordResetViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.ResetPassword_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            ResetPassword_PostOverride(callInfo, vm);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        [NonAction]
        partial void RequestPasswordResetOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.RequestPasswordResetViewModel vm);

        [NonAction]
        public override System.Web.Mvc.ActionResult RequestPasswordReset(BGC.Web.Areas.Administration.ViewModels.RequestPasswordResetViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RequestPasswordReset);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            RequestPasswordResetOverride(callInfo, vm);
            return callInfo;
        }

        [NonAction]
        partial void RequestPasswordReset_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.PasswordResetViewModel vm);

        [NonAction]
        public override System.Threading.Tasks.Task<System.Web.Mvc.ActionResult> RequestPasswordReset_Post(BGC.Web.Areas.Administration.ViewModels.PasswordResetViewModel vm)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.RequestPasswordReset_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "vm", vm);
            RequestPasswordReset_PostOverride(callInfo, vm);
            return System.Threading.Tasks.Task.FromResult(callInfo as ActionResult);
        }

        [NonAction]
        partial void LoginOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.LoginViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult Login(BGC.Web.Areas.Administration.ViewModels.LoginViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            LoginOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void Login_PostOverride(T4MVC_System_Web_Mvc_ActionResult callInfo, BGC.Web.Areas.Administration.ViewModels.LoginViewModel model);

        [NonAction]
        public override System.Web.Mvc.ActionResult Login_Post(BGC.Web.Areas.Administration.ViewModels.LoginViewModel model)
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Login_Post);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "model", model);
            Login_PostOverride(callInfo, model);
            return callInfo;
        }

        [NonAction]
        partial void LogOutOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult LogOut()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.LogOut);
            LogOutOverride(callInfo);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108