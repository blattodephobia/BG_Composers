using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface ISettingsService
    {
        /// <summary>
        /// Finds the highest priority setting with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        Setting FindSetting(string name);

        /// <summary>
        /// Sets a value 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        [OperationContract]
        void ConfigureSetting(string name, Parameter value);
    }
}
