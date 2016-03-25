using BGC.Core;
using BGC.Utilities;
using BGC.WebAPI.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI.Areas.Administration.Controllers
{
	public partial class AuthenticationController : AdministrationControllerBase
	{
		public SignInManager<AspNetUser, long> SignInManager { get; private set; }

		[AllowAnonymous]
		[HttpGet]
		public virtual ActionResult Login()
		{
			if (this.TempData.ContainsKey(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl))
			{
				this.TempData.Keep(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl);
			}
			return this.View();
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult Login(LoginViewModel model)
		{
			AspNetUser user = this.UserManager.FindByNameAsync(model.UserName).Result;
			bool success = user != null && this.SignInManager.PasswordSignIn(user.UserName, model.Password, false, false) == SignInStatus.Success;
			if (success)
			{
				object returnUrl;
				this.TempData.TryGetValue(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl, out returnUrl);
				return new RedirectResult(string.IsNullOrWhiteSpace(returnUrl as string) ? Url.Action(MVC.AdministrationArea.Account.Users()) : returnUrl as string);
			}
			else
			{
				return this.Login();
			}
		}

		public AuthenticationController(SignInManager<AspNetUser, long> signInManager)
		{
			this.SignInManager = signInManager;
		}
    }
}
