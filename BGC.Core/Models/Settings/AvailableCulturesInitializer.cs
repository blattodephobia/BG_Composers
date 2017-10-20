using CodeShield;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class AvailableCulturesInitializer : SettingInitializer<MultiCultureInfoSetting>
    {
        private IRepository<Composer> _composers;

        public AvailableCulturesInitializer(MultiCultureInfoSetting availableCulturesSetting, IRepository<Composer> composers) :
            base(availableCulturesSetting)
        {
            Shield.ArgumentNotNull(composers, nameof(composers)).ThrowOnError();

            _composers = composers;
        }

        protected override void InitializeInternal()
        {
            var composers = _composers.All().ToList();
            var distinctLanguages = new HashSet<string>(
                composers
                .SelectMany(composer => composer.GetArticles().Select(article => article.LanguageInternal))
                .Distinct());
            var availableLanguages = new HashSet<string>(distinctLanguages);
            
            foreach (Composer composer in _composers.All())
            {
                HashSet<string> localizedArticles = new HashSet<string>(composer.GetArticles().Select(a => a.LanguageInternal));
                foreach (string language in distinctLanguages)
                {
                    if (!localizedArticles.Contains(language))
                    {
                        availableLanguages.Remove(language);
                    }

                    if (availableLanguages.Count == 0)
                    {
                        return;
                    }
                }
            }

            Setting.Cultures = availableLanguages.Select(code => CultureInfo.GetCultureInfo(code));
        }
    }
}
