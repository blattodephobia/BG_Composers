using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public class ConfigurationBase
    {
        private ISettingsService _svc;
        private Dictionary<string, Setting> _cache = new Dictionary<string, Setting>();
        private string _prefix;
        private SettingsFactory _factory;

        private string GetFullSettingName(string name) =>
            string.IsNullOrWhiteSpace(_prefix)
            ? name
            : $"{_prefix}.{name}";

        private void Cache<T>(string settingName)
        {
            string fullName = GetFullSettingName(settingName);
            if (!_cache.ContainsKey(fullName))
            {
                Setting setting = _svc.ReadSetting(fullName);
                if (setting == null)
                {
                    setting = _factory.GetSetting<T>(fullName) as Setting;
                    _svc.WriteSetting(setting);
                }

                Shield.Assert(
                    value: fullName,
                    condition: setting is IParameter<T>,
                    exceptionProvider: (string name) => new InvalidOperationException($"The setting {name} doesn't implement {typeof(IParameter<T>)}"))
                    .ThrowOnError();

                _cache.Add(fullName, setting);
            }
        }

        private IParameter<T> GetSetting<T>(string name) => _cache[GetFullSettingName(name)] as IParameter<T>;

        protected void SetValue<T>(T value, [CallerMemberName] string settingName = null)
        {
            string fullSettingName = GetFullSettingName(settingName);
            Cache<T>(fullSettingName);

            Setting setting = _cache[fullSettingName];
            (setting as IParameter<T>).Value = value;
            _svc.WriteSetting(setting);
        }

        protected T ReadValue<T>([CallerMemberName] string settingName = null)
        {
            string fullSettingName = GetFullSettingName(settingName);
            Cache<T>(fullSettingName);

            var result = _cache[fullSettingName] as IParameter<T>;
            return result.Value;
        }

        public ConfigurationBase(ISettingsService settingsService, string implicitSettingsNamePrefix = null, SettingsFactory factory = null)
        {
            Shield.ArgumentNotNull(settingsService, nameof(settingsService)).ThrowOnError();

            _svc = settingsService;
            _prefix = implicitSettingsNamePrefix;
            _factory = factory ?? new SettingsFactory();
        }
    }
}
