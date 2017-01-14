using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using BGC.Web.Models;

namespace BGC.Web.Services
{
    public class DynamicMaxMinServiceProvider : IGeoLocationService
    {
        private readonly FileSystemWatcher _directoryWatcher;
        private MemoryStream _db;
        private MaxMindGeoLocationService _svc;
        private readonly string _fileName;
        private readonly object _svcLock = new object();

        private void Initialize(string filePath)
        {
            try
            {
                lock (_svcLock) using (Stream fileDb = File.OpenRead(filePath))
                {
                    _db?.Dispose();
                    _db = new MemoryStream();
                    fileDb.CopyTo(_db);
                    _db.Position = 0;
                    _svc = new MaxMindGeoLocationService(_db);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
            }
        }

        private void File_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name == _fileName)
            {
                Initialize(_directoryWatcher.Path);
            }
        }

        private void File_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name == _fileName)
            {
                Initialize(_directoryWatcher.Path);
            }
        }

        private void File_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.Name == _fileName)
            {
                Initialize(_directoryWatcher.Path);
            }
        }

        public DynamicMaxMinServiceProvider(string path)
        {
            _fileName = Path.GetFileName(path);
            Initialize(path);

            _directoryWatcher = new FileSystemWatcher(Path.GetDirectoryName(path))
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                EnableRaisingEvents = true,
                Filter = "*.mmdb"
            };
            _directoryWatcher.Changed += File_Changed;
            _directoryWatcher.Created += File_Created;
            _directoryWatcher.Renamed += File_Renamed;
        }

        public CountryInfo GetCountry(IPAddress ip)
        {
            lock (_svcLock)
            {
                return _svc.GetCountry(ip);
            }
        }
    }
}