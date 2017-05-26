using CodeShield;
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
        private bool _isSealed;

        private void CheckNotSealed()
        {
            Shield.AssertOperation(_isSealed, !_isSealed, $"This instance of {nameof(ApplicationProfile)} cannot be modified, because ot has been sealed.").ThrowOnError();
        }

        public bool IsSealed => _isSealed;

        private IEnumerable<CultureInfo> _supportedLanguages;
        public IEnumerable<CultureInfo> SupportedLanguages
        {
            get
            {
                return _supportedLanguages;
            }

            set
            {
                CheckNotSealed();
                _supportedLanguages = value;
            }
        }

        private string _localeRouteTokenName;
        public string LocaleRouteTokenName
        {
            get
            {
                return _localeRouteTokenName;
            }

            set
            {
                CheckNotSealed();
                _localeRouteTokenName = value;
            }
        }

        private string _localeCookieName;
        public string LocaleCookieName
        {
            get
            {
                return _localeCookieName;
            }

            set
            {
                CheckNotSealed();
                _localeCookieName = value;
            }
        }

        private string _localeKey;
        public string LocaleKey
        {
            get
            {
                return _localeKey;
            }

            set
            {
                CheckNotSealed();
                _localeKey = value;
            }
        }

        public bool IsLanguageSupported(CultureInfo language) => SupportedLanguages.Contains(language);

        public bool IsLanguageSupported(string language) => SupportedLanguages.Any(c => c.Name == language);

        /// <summary>
        /// Marks this instance of <see cref="ApplicationProfile"/> as sealed, which will cause an <see cref="InvalidOperationException"/> to
        /// be thrown if an attempt is made to modify any of its properties.
        /// </summary>
        public void Seal()
        {
            _isSealed = true;
        }

        public static ApplicationProfile FromApplicationSettings(IEnumerable<Setting> applicationSettings)
        {
            return new ApplicationProfile()
            {
                SupportedLanguages = new HashSet<CultureInfo>((applicationSettings.FirstOrDefault(s => s.Name == nameof(SupportedLanguages)) as CultureSupportSetting)?.SupportedCultures ?? Enumerable.Empty<CultureInfo>())
            };
        }
    }
}
