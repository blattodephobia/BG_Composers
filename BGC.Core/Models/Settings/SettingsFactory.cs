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
        public virtual IParameter<T> GetSetting<T>(string name) => (IParameter<T>)GetSetting(name, typeof(T));

        public virtual Setting GetSetting(string name, Type iParameterImplementationType)
        {
            Shield.ArgumentNotNull(name, nameof(name)).ThrowOnError();
            Shield.ArgumentNotNull(iParameterImplementationType, nameof(iParameterImplementationType)).ThrowOnError();

            Shield.AssertOperation(
                value: iParameterImplementationType,
                condition: SettingsMap.ContainsKey(iParameterImplementationType),
                message: $"There is no conversion defined between {iParameterImplementationType} and a corresponding implementation of {typeof(IParameter<>)}")
                .ThrowOnError();

            return SettingsMap[iParameterImplementationType].Invoke(name);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            Shield.ArgumentNotNull(serviceType, nameof(serviceType)).ThrowOnError();
            Shield.AssertOperation(serviceType, SettingsMap.ContainsKey(serviceType), $"There is no {nameof(Setting)} object supporting {serviceType}.").ThrowOnError();

            return SettingsMap[serviceType].Invoke("<unknown>");
        }
    }
}
