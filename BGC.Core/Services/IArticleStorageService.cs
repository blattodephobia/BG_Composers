using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface IArticleStorageService
    {
        [OperationContract]
        Guid StoreEntry(string text);

        [OperationContract]
        string GetEntry(Guid id);

        [OperationContract]
        void RemoveEntry(Guid id);

        [OperationContract]
        void UpdateEntry(Guid id, string text);
    }
}
