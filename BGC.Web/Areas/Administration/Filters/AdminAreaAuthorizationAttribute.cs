using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Public.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.Areas.Administration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AdminAreaAuthorizationAttribute : AuthorizeAttribute
    {
        private static IEnumerable<string> GetAvailablePermissions(HttpContextBase httpContext)
        {
            return (httpContext.User.Identity as ClaimsIdentity).FindAll(nameof(IPermission)).Select(c => c.Value);
        }

        private static PermissionsAttribute GetPermissionsFromDeclaringTypes(MethodInfo methodInfo)
        {
            Type currentDeclaringType = methodInfo.DeclaringType;
            PermissionsAttribute result = null;
            while (currentDeclaringType != null && result == null)
            {
                result = currentDeclaringType.GetCustomAttribute<PermissionsAttribute>();
                currentDeclaringType = currentDeclaringType.DeclaringType;
            }

            return result;
        }

        internal static PermissionsAttribute GetRequiredPermissions(MethodInfo action)
        {
            PermissionsAttribute attr = action.GetCustomAttribute<PermissionsAttribute>() ?? GetPermissionsFromDeclaringTypes(action);
            return attr;
        }

        internal bool HasNecessaryPermissions(IEnumerable<string> availablePermissions, PermissionsAttribute requiredPermissions)
        {
            if (!availablePermissions.Any())
            {
                return false;
            }

            return requiredPermissions?.IsAuthorized(availablePermissions) ?? true;
        }

        internal void HandleUnauthorizedRequestInternal(AuthorizationContext filterContext, string loginUrl)
        {
            if (filterContext.HttpContext.User?.Identity.IsAuthenticated ?? false)
            {
                filterContext.Result = new ViewResult()
                {
                    ViewName = MVC.Shared.Views.Error,
                    ViewData = new ViewDataDictionary(new ErrorViewModel(filterContext.HttpContext.Response))
                };
                filterContext.Controller = (ControllerBase)ControllerBuilder.Current.GetControllerFactory().CreateController(new RequestContext(filterContext.HttpContext, filterContext.RouteData), MVC.Public.Main.Name);
            }
            else
            {
                HttpRequestBase request = filterContext.RequestContext.HttpContext.Request;
                string query = request.QueryString.Keys.Cast<string>().ToStringAggregate(key => $"{key}={request.QueryString[key]}", "&");
                string returnUrl = string.IsNullOrEmpty(query)
                    ? request.CurrentExecutionFilePath :
                    $"{request.CurrentExecutionFilePath}?{query}";

                if (filterContext.Controller.TempData.ContainsKey(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl))
                {
                    filterContext.Controller.TempData.Remove(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl);
                }
                filterContext.Controller.TempData.Add(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl, returnUrl);

                filterContext.Result = new RedirectResult(loginUrl);
            }
        }


        private string GetActionUrl(RequestContext context, ActionResult action)
        {
            return new UrlHelper(context).ActionAbsolute(action);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext) => HandleUnauthorizedRequestInternal(filterContext, GetActionUrl(filterContext.RequestContext, MVC.Administration.Authentication.Login()));

        private MethodInfo ResolveAction(HttpContextBase httpContext)
        {
            string controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            return MVC.Administration.ControllerActions[controller][action, httpContext.Request.HttpMethod];
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (base.AuthorizeCore(httpContext))
            {
                MethodInfo actionToBeExecuted = ResolveAction(httpContext);

                bool isAuthorized = HasNecessaryPermissions(
                    availablePermissions: GetAvailablePermissions(httpContext),
                    requiredPermissions: GetRequiredPermissions(actionToBeExecuted));

                if (!isAuthorized)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    httpContext.Response.StatusDescription = LocalizationKeys.Global.Forbidden;
                }

                return isAuthorized;
            }
            else
            {
                return false;
            }
        }

        private AdminAreaAuthorizationAttribute(IEnumerable<string> rolesCollection)
        {
            Roles = rolesCollection.ToStringAggregate(",");
        }

        public AdminAreaAuthorizationAttribute(params string[] roles) :
            this(rolesCollection: roles)
        {
        }

        public AdminAreaAuthorizationAttribute() :
            base()
        {
        }
    }
}