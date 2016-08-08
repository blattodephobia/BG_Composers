using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface IArticleContentService
    {
        [OperationContract]
        Guid StoreEntry(string content);

        [OperationContract]
        string GetEntry(Guid id);

        [OperationContract]
        void RemoveEntry(Guid id);

        [OperationContract]
        void UpdateEntry(Guid id, string content);
    }
}
