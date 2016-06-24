using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface IDataStorageService<T>
    {
        [OperationContract]
        Guid StoreEntry(T content);

        [OperationContract]
        T GetEntry(Guid id);

        [OperationContract]
        void RemoveEntry(Guid id);

        [OperationContract]
        void UpdateEntry(Guid id, T content);
    }
}
