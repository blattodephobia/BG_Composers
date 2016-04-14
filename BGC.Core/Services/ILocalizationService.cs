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
        /// Gets a translation corresponding to the <paramref name="key"/> given in the culture
        /// specified by the <see cref="Culture"/> property.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Localize(string key);

        /// <summary>
        /// Gets a translation corresponding to the <paramref name="key"/> overriding the <see cref="Culture"/>
        /// property.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="culture">The culture for the translation string to be returned.</param>
        /// <returns></returns>
        string Localize(string key, CultureInfo culture);

        /// <summary>
        /// Gets or sets the culture in which translations will be given.
        /// </summary>
        CultureInfo Culture { get; set; }
    }
}
