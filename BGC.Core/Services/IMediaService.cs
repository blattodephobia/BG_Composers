using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Core.Services
{
    [ServiceContract]
    public interface IMediaService
    {
        [OperationContract]
        Guid AddMedia(ContentType contentType, Stream data, string fileName, Guid? articleId = null);

        [OperationContract]
        MediaTypeInfo GetMedia(Guid mediaId);
    }
}
