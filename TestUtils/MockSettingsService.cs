using BGC.Core;
using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUtils
{
    public abstract class MockSettingsService
    {
        private readonly IList<Setting> _settingsRepo;

        public MockSettingsService(IList<Setting> settingsRepo = null)
        {
            _settingsRepo = settingsRepo ?? new List<Setting>();
        }

        public abstract Setting ReadSetting(string name);

        public T ReadSetting<T>(string name) where T : Setting => (T)ReadSetting(name);
    }
}
