using BGC.Core;
using BGC.Utilities;
using BGC.Web.Areas.Public.ViewModels;
using RouteDebug;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BGC.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class WebApiApplication : System.Web.HttpApplication
	{
		public class DataKeys
		{
			public class AdministrationArea
			{
				public static readonly string LoginSuccessReturnUrl = $"{nameof(AdministrationArea)}.{nameof(LoginSuccessReturnUrl)}";
			}

            public class Global
            {
                public static readonly string ErrorViewModelKey = $"{nameof(Global)}.{nameof(ErrorViewModel)}";
            }
		}

        private static ActionResult SelectErrorResult(RequestContext requestContext)
        {
            ActionResult responseResult = requestContext.HttpContext.Request.IsAjaxRequest() ?
                new JsonResult() // when the request is ajax the system can automatically handle a mistake with a JSON response. then overwrites the default response
                {
                    Data = new { success = false, serverError = (int)HttpStatusCode.InternalServerError },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                } as ActionResult
                :
                new ViewResult()
                {
                    ViewData = new ViewDataDictionary(new ErrorViewModel(
                        statusCode: HttpStatusCode.InternalServerError,
                        description: LocalizationKeys.Global.InternalServerError)),
                    ViewName = MVC.Shared.Views.Error
                } as ActionResult;
            return responseResult;
        }

		protected void Application_Start()
		{
			WebApiConfig.Register(GlobalConfiguration.Configuration);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			AreaRegistration.RegisterAllAreas();
		}

        protected void Application_Error()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
            {
                httpContext.ClearError();
                httpContext.Response.Clear();

                IControllerFactory factory = ControllerBuilder.Current.GetControllerFactory();
                RequestContext requestContext = ((MvcHandler)httpContext.CurrentHandler).RequestContext;
                ControllerBase controller = factory.CreateController(requestContext, MVC.Public.Main.Name) as ControllerBase;
                controller.ControllerContext = new ControllerContext(requestContext, controller);
                ActionResult responseResult = SelectErrorResult(requestContext);
                responseResult.ExecuteResult(controller.ControllerContext);

                httpContext.Response.End();
            }
        }
    }
}