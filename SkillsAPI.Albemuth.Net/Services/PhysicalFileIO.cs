using SkillsAPI.Albemuth.Net.Contracts;
using System.Collections.Generic;
using System.IO;

namespace SkillsAPI.Albemuth.Net.Services
{
    public class PhysicalFileIO : IFileIO
    {
        private readonly IAppSettings appSettings;

        public PhysicalFileIO(IAppSettings appSettings)
        {
            this.appSettings = appSettings;
        }

        public IEnumerable<string> GetAllFiles()
        {
            return Directory.EnumerateFiles(appSettings.DataFolder);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public StreamReader OpenFileStreamReader(string path)
        {
            return File.OpenText(path);
        }
    }
}

