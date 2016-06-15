using BGC.Core.Services;
using CodeShield;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    internal class FileSystemArticleStorageService : IArticleStorageService
    {
        private DirectoryInfo directory;

        protected FileInfo GuidToFileName(Guid guid)
        {
            return new FileInfo(directory.FullName + guid.ToString("D").ToUpperInvariant());
        }

        protected Guid FileNameToGuid(FileInfo file)
        {
            return Guid.Parse(file.Name);
        }

        public FileSystemArticleStorageService(DirectoryInfo directory)
        {
            this.directory = directory.ArgumentNotNull(nameof(directory)).GetValueOrThrow();
        }

        public string GetEntry(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveEntry(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid StoreEntry(string text)
        {
            throw new NotImplementedException();
        }

        public void UpdateEntry(Guid id, string text)
        {
            throw new NotImplementedException();
        }
    }
}
