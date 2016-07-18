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
    internal class FileSystemMediaStorageService : FileSystemStorageService<Stream>, IMediaStorageService
    {
        protected IRepository<MediaTypeInfo> MetaDataRepository { get; private set; }
        protected IRepository<ComposerArticle> ArticleRepository { get; private set; }
        
        public Guid AddMedia(ContentType contentType, Stream data)
        {
            return AddMedia(contentType, data, default(Guid));
        }

        public MediaTypeInfo GetMedia(Guid guid)
        {
            MediaTypeInfo result = MetaDataRepository.All().FirstOrDefault(media => media.StorageId == guid);
            result.Content = GuidToFileName(guid).OpenRead();
            return result;
        }

        public override Stream GetEntry(Guid id)
        {
            return GetMedia(id).Content;
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
            using (BufferedStream wrapper = new BufferedStream(data))
            using (Stream storage = GuidToFileName(id).Open(FileMode.OpenOrCreate))
            {
                wrapper.CopyTo(storage);
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

        public Guid AddMedia(ContentType contentType, Stream data, Guid articleId)
        {
            Guid storageId = StoreEntry(data);
            var mediaEntry = new MediaTypeInfo() { MimeType = contentType, StorageId = storageId };
            MetaDataRepository.Insert(mediaEntry);
            ArticleRepository.All().FirstOrDefault(article => article.StorageId == articleId)?.Media.Add(mediaEntry);
            SaveAll();
            return storageId;

        }

        public FileSystemMediaStorageService(
            [Dependency(ServiceLayerDependencyRegistration.DefaultMediaStorageDirectoryKey)] DirectoryInfo storageDir,
            IRepository<MediaTypeInfo> metaDataRepository,
            IRepository<ComposerArticle> articleRepository) :
            base(storageDir)
        {
            MetaDataRepository = metaDataRepository;
            ArticleRepository = articleRepository;
        }
    }
}
