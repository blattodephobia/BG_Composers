using BGC.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGC.Services
{
    internal class FileSystemImageStorageService : FileSystemDataStorageService
    {
        protected override string GetEntryInternal(Guid id)
        {
            using (Stream read = GuidToFileName(id).OpenRead())
            {
                byte[] data = new byte[read.Length];
                read.ReadAsync(data, 0, data.Length);
                return Convert.ToBase64String(data);
            }
        }

        protected override Guid StoreEntryInternal(string text)
        {
            Guid id = Guid.NewGuid();
            using (Stream write = GuidToFileName(id).OpenWrite())
            {
                byte[] data = Convert.FromBase64String(text);
                write.Write(data, 0, data.Length);
                return id;
            }
        }

        protected override void UpdateEntryInternal(Guid id, string text)
        {
            base.UpdateEntryInternal(id, text);
        }

        public FileSystemImageStorageService(DirectoryInfo storageDir) :
            base(storageDir)
        {
        }
    }
}
