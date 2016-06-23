using BGC.Core.Services;
using CodeShield;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    internal class FileSystemDataStorageService : IDataStorageService
    {
        private DirectoryInfo directory;

        protected FileInfo GuidToFileName(Guid guid)
        {
            return new FileInfo($"{directory.FullName}\\{guid.ToString("D").ToUpperInvariant()}");
        }

        protected Guid FileNameToGuid(FileInfo file)
        {
            return Guid.Parse(file.Name);
        }

        protected virtual string GetEntryInternal(Guid id)
        {
            using (StreamReader read = GuidToFileName(id).OpenText())
            {
                return read.ReadToEnd();
            }
        }

        protected virtual Guid StoreEntryInternal(string text)
        {
            Guid newId = Guid.NewGuid();
            using (StreamWriter write = new StreamWriter(GuidToFileName(newId).OpenWrite()))
            {
                write.Write(text);
                return newId;
            }
        }

        protected virtual void UpdateEntryInternal(Guid id, string text)
        {
            using (StreamWriter write = new StreamWriter(GuidToFileName(id).OpenWrite()))
            {
                write.Write(text);
            }
        }

        public FileSystemDataStorageService([Dependency(ServiceLayerDependencyRegistration.DefaultDataStorageDirectoryKey)] DirectoryInfo storageDir)
        {
            this.directory = storageDir.ArgumentNotNull(nameof(directory)).GetValueOrThrow();
        }

        public string GetEntry(Guid id) => GetEntryInternal(id);

        public void RemoveEntry(Guid id)
        {
            GuidToFileName(id).Delete();
        }

        public Guid StoreEntry(string text) => StoreEntryInternal(text);

        public void UpdateEntry(Guid id, string text) => UpdateEntryInternal(id, text);
    }
}
