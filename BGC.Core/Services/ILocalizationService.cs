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
        /// <summary>
        /// Gets or sets the default culture in which translations will be given.
        /// </summary>
        CultureInfo DefaultCulture { get; set; }      
          
        /// <summary>
        /// Gets a translation associated with a key.
        /// </summary>
        /// <param name="key">The <paramref name="key"/> that identifies a localized string.</param>
        /// <param name="culture">The language to localize the key in. If <paramref name="culture"/> is null, the value of the <see cref="DefaultCulture"/> property will be used.</param>
        /// <returns>A string localized in the specified language or the <paramref name="key"/>, if there is no applicable translation.</returns>
        string Localize(string key, CultureInfo culture = null);

        /// <summary>
        /// Gets the set of a language's graphemes.
        /// </summary>
        /// <param name="useUpperCase">Whether to return the graphemes in upper case where applicable.</param>
        /// <param name="culture">The language for which an alphabet will be returned. Use null to imply the <see cref="DefaultCulture"/></param>
        /// <returns></returns>
        char[] GetAlphabet(bool useUpperCase = true, CultureInfo culture = null);
    }
}
