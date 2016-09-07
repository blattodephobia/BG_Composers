using BGC.Core;
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

namespace BGC.Web.Areas.Administration.Controllers
{
	public partial class AuthenticationController : AdministrationControllerBase
	{
		public SignInManager<BgcUser, long> SignInManager { get; private set; }

		[AllowAnonymous]
		[HttpGet]
		public virtual ActionResult Login()
		{
			if (TempData.ContainsKey(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl))
			{
				TempData.Keep(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl);
			}
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult Login(LoginViewModel model)
		{
			if (SignInManager.PasswordSignIn(model.UserName, model.Password, model.Remember, false) == SignInStatus.Success)
			{
				object returnUrl;
				TempData.TryGetValue(WebApiApplication.TempDataKeys.AdministrationArea.LoginSuccessReturnUrl, out returnUrl);
				return new RedirectResult(string.IsNullOrWhiteSpace(returnUrl as string) ? Url.Action(MVC.AdministrationArea.Account.Users()) : returnUrl as string);
			}
			else
			{
				return Login();
			}
		}

		public AuthenticationController(SignInManager<BgcUser, long> signInManager)
		{
			SignInManager = signInManager;
		}
    }
}
