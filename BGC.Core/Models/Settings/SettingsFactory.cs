using CodeShield;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core
{
    public partial class SettingsFactory : IServiceProvider
    {
        public virtual IParameter<T> GetSetting<T>(string name)
        {
            Shield.ArgumentNotNull(name, nameof(name)).ThrowOnError();
            Shield.AssertOperation(typeof(T), SettingsMap.ContainsKey(typeof(T)), $"There is no conversion defined between {typeof(T)} and a corresponding implementation of {typeof(IParameter<>)}").ThrowOnError();

            return SettingsMap[typeof(T)].Invoke(name) as IParameter<T>;
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            Shield.ArgumentNotNull(serviceType, nameof(serviceType)).ThrowOnError();
            Shield.AssertOperation(serviceType, SettingsMap.ContainsKey(serviceType), $"There is no {nameof(Setting)} object supporting {serviceType}.").ThrowOnError();

            return Activator.CreateInstance(SettingsMap[serviceType].Method.ReturnType);
        }
    }
}
