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
        [OperationContract]
        Setting FindSetting(string name);

        [OperationContract]
        void ConfigureSetting(string name, Parameter value);

        [OperationContract]
        void ClearSetting(string name);
    }
}
