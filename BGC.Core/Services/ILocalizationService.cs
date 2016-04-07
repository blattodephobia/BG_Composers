using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    public interface ILocalizationService
    {
        string Localize(string key);

        CultureInfo Culture { get; set; }
    }
}
