using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core;
using System.Net.Mime;

namespace BGC.Services
{
    internal class FileSystemMediaStorageService : FileSystemStorageService<Stream>, IMediaStorageService
    {
        public Guid AddMedia(ContentType contentType, Stream data)
        {
            throw new NotImplementedException();
        }

        public MediaTypeInfo GetMedia(Guid guid)
        {
            throw new NotImplementedException();
        }

        public override Stream GetEntry(Guid id)
        {
            return GuidToFileName(id).OpenRead();
        }

        public override Guid StoreEntry(Stream text)
        {
            Guid targetId = Guid.NewGuid();
            using (BufferedStream wrapper = new BufferedStream(text))
            using (Stream storage = GuidToFileName(targetId).OpenWrite())
            {
                wrapper.CopyTo(storage);
            }

            return targetId;
        }

        public override void UpdateEntry(Guid id, Stream data)
        {
            using (BufferedStream wrapper = new BufferedStream(data))
            using (Stream storage = GuidToFileName(id).Open(FileMode.OpenOrCreate))
            {
                wrapper.CopyTo(storage);
            }
        }

        public FileSystemMediaStorageService(DirectoryInfo storageDir) :
            base(storageDir)
        {
        }
    }
}
