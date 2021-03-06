﻿using BGC.Web.Areas.Administration.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.Areas.Administration
{
	public class AdministrationAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return MVC.Administration.Name; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context
                .Routes
                .MapRoute(
                    name: $"{AreaName}",
                    url: $"areas/{AreaName}/{{controller}}/{{action}}",
                    namespaces: new[] { typeof(AdministrationControllerBase).Namespace })
                .DataTokens.Add("area", AreaName);
        }
	}
}