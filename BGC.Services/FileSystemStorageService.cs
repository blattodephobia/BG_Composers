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
    internal abstract class FileSystemStorageService<T> : DbServiceBase
    {
        private DirectoryInfo directory;

        public abstract T GetEntry(Guid id);

        public abstract Guid StoreEntry(T data);

        public abstract void UpdateEntry(Guid id, T data);

        public virtual void RemoveEntry(Guid id)
        {
            GuidToFileName(id).Delete();
        }

        protected FileInfo GuidToFileName(Guid guid)
        {
            return new FileInfo($"{directory.FullName}\\{guid.ToString("D").ToUpperInvariant()}");
        }

        protected Guid FileNameToGuid(FileInfo file)
        {
            return Guid.Parse(file.Name);
        }

        protected FileSystemStorageService(DirectoryInfo storageDir)
        {
            this.directory = storageDir.ArgumentNotNull(nameof(directory)).GetValueOrThrow();
        }
    }
}
