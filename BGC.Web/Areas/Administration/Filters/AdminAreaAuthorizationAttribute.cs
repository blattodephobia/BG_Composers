using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.Areas.Administration
{
	public class AdminAreaAuthorizationAttribute : AuthorizeAttribute
	{
		protected string GetActionUrl(RequestContext context, ActionResult action)
		{
			return new UrlHelper(context).ActionAbsolute(action);
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			string returnUrl = filterContext.RequestContext.HttpContext.Request.CurrentExecutionFilePath;
			string loginUrl = this.GetActionUrl(filterContext.RequestContext, MVC.AdministrationArea.Authentication.Login());
			
			if (filterContext.Controller.TempData.ContainsKey(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl))
			{
				filterContext.Controller.TempData.Remove(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl);
			}
			
			filterContext.Controller.TempData.Add(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl, returnUrl);
			filterContext.Result = new RedirectResult(loginUrl);
		}
	}
}