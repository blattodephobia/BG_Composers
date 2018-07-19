using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core;
using System.Net.Mime;
using Microsoft.Practices.Unity;
using BGC.Data;

namespace BGC.Services
{
    internal class FileSystemMediaService : FileSystemStorageService<Stream>, IMediaService
    {
        private readonly IMediaTypeInfoRepository _metaDataRepository;

        public MultimediaContent GetMedia(Guid storageId)
        {
            try
            {
                MediaTypeInfo metadata = _metaDataRepository.Find(storageId);
                var result = metadata != null
                    ? new MultimediaContent(GuidToFileName(storageId)?.OpenRead(), metadata)
                    : null;

                return result;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public override Stream GetEntry(Guid id)
        {
            return GetMedia(id)?.Data;
        }

        public override Guid StoreEntry(Stream data)
        {
            Guid targetId = Guid.NewGuid();
            using (BufferedStream wrapper = new BufferedStream(data))
            using (Stream storage = GuidToFileName(targetId).OpenWrite())
            {
                wrapper.CopyTo(storage);
            }

            return targetId;
        }

        public override void UpdateEntry(Guid id, Stream data)
        {
            using (Stream storage = GuidToFileName(id).Open(FileMode.OpenOrCreate))
            {
                data.CopyTo(storage);
            }
        }

        public override void RemoveEntry(Guid id)
        {
            MediaTypeInfo entry = _metaDataRepository.Find(id);
            if (entry != null)
            {
                _metaDataRepository.Delete(entry);
                base.RemoveEntry(id);
                _metaDataRepository.SaveChanges();
            }
        }

        public Guid AddMedia(ContentType contentType, Stream data, string fileName)
        {
            Guid storageId = StoreEntry(data);
            var mediaEntry = new MediaTypeInfo(fileName, contentType) { StorageId = storageId };
            _metaDataRepository.AddOrUpdate(mediaEntry);

            _metaDataRepository.SaveChanges();

            return storageId;

        }

        public FileSystemMediaService(
            [Dependency(ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey)] DirectoryInfo storageDir,
            IMediaTypeInfoRepository metaDataRepository) :
            base(storageDir)
        {
            _metaDataRepository = metaDataRepository;
        }
    }
}
