using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
            if (!_cache.ContainsKey(settingName))
            {
                Setting setting = _svc.ReadSetting(settingName);
                if (setting == null)
                {
                    setting = _factory.GetSetting<T>(settingName) as Setting;
                    _svc.WriteSetting(setting);
                }

                Shield.Assert(
                    value: settingName,
                    condition: setting is IParameter<T>,
                    exceptionProvider: (string name) => new InvalidOperationException($"The setting {name} doesn't implement {typeof(IParameter<T>)}"))
                    .ThrowOnError();

                _cache.Add(settingName, setting);
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

        public IEnumerable<string> GetSettingNames() => GetType().GetProperties().Select(p => GetFullSettingName(p.Name));

        public string GetDescriptionFor(string settingName) => GetType().GetProperty(settingName)?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";

        public ConfigurationBase(ISettingsService settingsService, string implicitSettingsNamePrefix = null, SettingsFactory factory = null)
        {
            Shield.ArgumentNotNull(settingsService, nameof(settingsService)).ThrowOnError();

            _svc = settingsService;
            _prefix = implicitSettingsNamePrefix;
            _factory = factory ?? new SettingsFactory();
        }
    }
}
