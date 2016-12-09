using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BGC.Web.ViewModels
{
    public abstract partial class ViewModelBase
    {
        protected ViewModelBase()
        {
            ErrorMessages = Enumerable.Empty<string>();
        }

        private bool _renderErrorsOnly;
        /// <summary>
        /// Specifies whether the view should render only error messages or the rest of its HTML, in addition to error messages.
        /// If <see cref="ErrorMessages"/> is empty, this property will always return false. 
        /// </summary>
        public bool RenderErrorsOnly
        {
            get
            {
                return _renderErrorsOnly && (ErrorMessages?.Any() ?? false);
            }

            set
            {
                _renderErrorsOnly = value;
            }
        }

        public ViewModelBase WithModelStateErrors(ModelStateDictionary modelState)
        {
            if (modelState != null)
            {
                var newErrorMessages = new List<string>(modelState?.Values.SelectMany(m => m.Errors).Select(err => err.ErrorMessage));
                newErrorMessages.AddRange(ErrorMessages);
                ErrorMessages = newErrorMessages;
            }

            return this;
        }

        private IEnumerable<string> _errorMessages;
        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return _errorMessages ?? (_errorMessages = Enumerable.Empty<string>());
            }

            set
            {
                _errorMessages = value;
            }
        }
    }
}