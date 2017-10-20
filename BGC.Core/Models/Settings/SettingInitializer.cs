using CodeShield;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public abstract class SettingInitializer<T> where T : Setting
    {
        public T Setting { get; private set; }

        protected SettingInitializer(T setting)
        {
            Shield.ArgumentNotNull(setting, nameof(setting)).ThrowOnError();
            Setting = setting;
        }

        protected abstract void InitializeInternal();

        public virtual bool IsInitialized { get; private set; }

        public T Initialize()
        {
            if (!IsInitialized)
            {
                InitializeInternal();
                IsInitialized = true;
            }

            return Setting;
        }

        public static implicit operator T(SettingInitializer<T> setting)
        {
            return setting.Setting;
        }
    }
}
