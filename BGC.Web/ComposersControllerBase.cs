using BGC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.WebAPI
{
	public abstract class ComposersControllerBase : Controller
	{
		public IUnitOfWork UnitOfWork { get; protected set; }

		protected ComposersControllerBase()
		{
		}

		protected ComposersControllerBase(IUnitOfWork unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
		}
	}
}