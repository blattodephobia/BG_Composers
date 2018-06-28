using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    public interface ISearchService
    {
        IEnumerable<SearchResult> Search(string query, CultureInfo locale);
    }
}
