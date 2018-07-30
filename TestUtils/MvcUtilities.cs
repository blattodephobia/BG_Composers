using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestUtils
{
    public static class MvcUtilities
    {
        public static TViewModel ExtractViewModel<TViewModel>(ActionResult action)
        {
            if (!(action is ViewResult))
            {
                throw new InvalidOperationException("Couldn't extract model from ActionResult");
            }

            TViewModel result = (TViewModel)(action as ViewResult)?.Model;
            return result;
        } 
    }
}
