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
        /// Finds the highest priority setting that the service can currently access by using the given setting's name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        Setting ReadSetting(string name);
    }
}
