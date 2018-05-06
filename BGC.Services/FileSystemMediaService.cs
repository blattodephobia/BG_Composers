﻿using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BGC.Core;
using System.Net.Mime;
using Microsoft.Practices.Unity;

namespace BGC.Services
{
    internal class FileSystemMediaService : FileSystemStorageService<Stream>, IMediaService
    {
        protected IRepository<MediaTypeInfo> MetaDataRepository { get; private set; }

        public MultimediaContent GetMedia(Guid guid)
        {
            MediaTypeInfo metadata = MetaDataRepository.All().FirstOrDefault(media => media.StorageId == guid);
            var result = metadata != null
                ? new MultimediaContent(GuidToFileName(guid)?.OpenRead(), metadata)
                : null;

            return result;
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
            MediaTypeInfo entry = MetaDataRepository.All().FirstOrDefault(media => media.StorageId == id);
            if (entry != null)
            {
                MetaDataRepository.Delete(entry);
                base.RemoveEntry(id);
                SaveAll();
            }
        }

        public Guid AddMedia(ContentType contentType, Stream data, string fileName)
        {
            Guid storageId = StoreEntry(data);
            var mediaEntry = new MediaTypeInfo(fileName, contentType) { StorageId = storageId };
            MetaDataRepository.Insert(mediaEntry);

            SaveAll();

            return storageId;

        }

        public FileSystemMediaService(
            [Dependency(ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey)] DirectoryInfo storageDir,
            IRepository<MediaTypeInfo> metaDataRepository) :
            base(storageDir)
        {
            MetaDataRepository = metaDataRepository;
        }
    }
}
