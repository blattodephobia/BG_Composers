using BGC.Core;
using BGC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Returns an array of <see cref="IPermission"/> collections which represent the permissions required for a user to perform an action.
        /// A user can perform an action if they have all or more than the necessary permissions in either one of the collections from the array.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static IEnumerable<Type>[] GetRequiredPermissionClauses(MethodInfo action)
        {
            return null;// action.GetCustomAttributes<PermissionsAttribute>().Select(attr => attr.PermissionTypes.Where(p => typeof(IPermission).IsAssignableFrom(p))).ToArray();
        }

        internal bool HasNecessaryPermissions(IEnumerable<string> availablePermissions, PermissionsAttribute requiredPermissions)
        {
            if (!availablePermissions.Any())
            {
                return false;
            }
            
            return requiredPermissions?.IsAuthorized(availablePermissions) ?? true;
        }

		private string GetActionUrl(RequestContext context, ActionResult action)
		{
			return new UrlHelper(context).ActionAbsolute(action);
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			string returnUrl = filterContext.RequestContext.HttpContext.Request.CurrentExecutionFilePath;
			string loginUrl = GetActionUrl(filterContext.RequestContext, MVC.Administration.Authentication.Login());
			
			if (filterContext.Controller.TempData.ContainsKey(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl))
			{
				filterContext.Controller.TempData.Remove(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl);
			}
			
			filterContext.Controller.TempData.Add(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl, returnUrl);
			filterContext.Result = new RedirectResult(loginUrl);
		}

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
                    requiredPermissions: actionToBeExecuted.GetCustomAttribute<PermissionsAttribute>());

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