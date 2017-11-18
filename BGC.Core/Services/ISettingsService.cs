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
        /// Returns null, if no <see cref="Setting"/> with the given <paramref name="name"/> exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        Setting ReadSetting(string name);

        /// <summary>
        /// Stores or updates a <see cref="Setting"/>.
        /// </summary>
        /// <param name="setting"></param>
        void WriteSetting(Setting setting);

        /// <summary>
        /// Deletes the specified <see cref="Setting"/> from the service's backing store. The <see cref="Setting"/>'s <see cref="Setting.Priority"/> property
        /// is taken into account.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="priority"></param>
        void DeleteSetting(Setting setting);
    }
}
