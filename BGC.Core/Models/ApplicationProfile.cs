using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ApplicationProfile
    {
        public IEnumerable<CultureInfo> SupportedLanguages { get; private set; }

        public bool IsLanguageSupported(CultureInfo language) => SupportedLanguages.Contains(language);

        public bool IsLanguageSupported(string language) => SupportedLanguages.Any(c => c.Name == language);

        public ApplicationProfile(IEnumerable<Setting> applicationSettings)
        {
            SupportedLanguages = new HashSet<CultureInfo>((applicationSettings.FirstOrDefault(s => s.Name == nameof(SupportedLanguages)) as CultureSupportSetting)?.SupportedCultures ?? Enumerable.Empty<CultureInfo>());
        }
    }
}
