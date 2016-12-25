using BGC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGC.Web.Areas.Public.ViewModels
{
    public class SearchResultViewModel : ViewModelBase
    {
        private Dictionary<Guid, string> _results;
        /// <summary>
        /// Contains the IDs of found articles and the names of the composers.
        /// </summary>
        public Dictionary<Guid, string> Results
        {
            get
            {
                return _results ?? (_results = new Dictionary<Guid, string>());
            }

            set
            {
                _results = value;
            }
        }
    }
}