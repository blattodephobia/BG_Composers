﻿using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Administration.ViewModels;
using BGC.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BGC.Web.Areas.Administration.Controllers
{
	public partial class AuthenticationController : AdministrationControllerBase
	{
        private string GetAdminHomePageUrl()
        {
            string result = Url.Action(MVC.Administration.Account.ActionNames.Activities, MVC.Administration.Account.Name);
            return result;
        }

		[AllowAnonymous]
		[HttpGet]
		public virtual ActionResult Login(LoginViewModel model = null)
		{
            if (User != null)
            {
                return new RedirectResult(GetAdminHomePageUrl());
            }
            else
            {
                if (TempData.ContainsKey(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl))
                {
                    TempData.Keep(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl);
                }
                return View(model);
            }
		}

		[AllowAnonymous]
		[HttpPost]
        [ActionName(nameof(Login))]
		public virtual ActionResult Login_Post(LoginViewModel model)
		{
            if (User != null)
            {
                return new HttpUnauthorizedResult("Duplicate login actions are not allowed");
            }

			BgcUser user = UserManager.FindByNameAsync(model.UserName).Result;
			bool success = user != null && SignInManager.PasswordSignIn(user.UserName, model.Password, false, false) == SignInStatus.Success;
			if (success)
			{
				object returnUrl;
				TempData.TryGetValue(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl, out returnUrl);
				return new RedirectResult(string.IsNullOrWhiteSpace(returnUrl as string) ? GetAdminHomePageUrl() : returnUrl as string);
			}
			else
			{
			    if (TempData.ContainsKey(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl))
			    {
				    TempData.Keep(WebApiApplication.DataKeys.AdministrationArea.LoginSuccessReturnUrl);
			    }
				return Login(new LoginViewModel() { AuthenticationFailure = true });
			}
		}

        [AllowAnonymous]
        public virtual ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction(MVC.Administration.Authentication.Login());
        }
    }
}
