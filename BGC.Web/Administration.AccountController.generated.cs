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
    public partial class AccountController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected AccountController(Dummy d) { }

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

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public AccountController Actions { get { return MVC.Administration.Account; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "Administration";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Account";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Account";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Activities = "Activities";
            public readonly string ChangePassword = "ChangePassword";
            public readonly string ChangePassword_Post = nameof(ChangePassword);
            public readonly string ResetPassword = "ResetPassword";
            public readonly string ResetPassword_Post = nameof(ResetPassword);
            public readonly string RequestPasswordReset = "RequestPasswordReset";
            public readonly string RequestPasswordReset_Post = nameof(RequestPasswordReset);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Activities = "Activities";
            public const string ChangePassword = "ChangePassword";
            public const string ChangePassword_Post = nameof(ChangePassword);
            public const string ResetPassword = "ResetPassword";
            public const string ResetPassword_Post = nameof(ResetPassword);
            public const string RequestPasswordReset = "RequestPasswordReset";
            public const string RequestPasswordReset_Post = nameof(RequestPasswordReset);
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
                public readonly string Activities = "Activities";
                public readonly string ChangePassword = "ChangePassword";
                public readonly string RequestPasswordReset = "RequestPasswordReset";
                public readonly string ResetPassword = "ResetPassword";
                public readonly string Users = "Users";
            }
            public readonly string Activities = "~/Areas/Administration/Views/Account/Activities.cshtml";
            public readonly string ChangePassword = "~/Areas/Administration/Views/Account/ChangePassword.cshtml";
            public readonly string RequestPasswordReset = "~/Areas/Administration/Views/Account/RequestPasswordReset.cshtml";
            public readonly string ResetPassword = "~/Areas/Administration/Views/Account/ResetPassword.cshtml";
            public readonly string Users = "~/Areas/Administration/Views/Account/Users.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_AccountController : BGC.Web.Areas.Administration.Controllers.AccountController
    {
        public T4MVC_AccountController() : base(Dummy.Instance) { }

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

    }
}

#endregion T4MVC
#pragma warning restore 1591, 3008, 3009, 0108
