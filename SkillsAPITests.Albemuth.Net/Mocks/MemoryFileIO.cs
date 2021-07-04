using SkillsAPI.Albemuth.Net.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SkillsAPITests.Albemuth.Net.Mocks
{
    internal class MemoryFileIO : IFileIO
    {
        public Dictionary<string, string> FileContents { get; } = new Dictionary<string, string>();

        public bool FileExists(string path) => FileContents.ContainsKey(path);

        public IEnumerable<string> GetAllFiles()
        {
            return FileContents.Keys;
        }

        public StreamReader OpenFileStreamReader(string path)
        {
            if (!FileContents.TryGetValue(path, out string contents))
            {
                throw new Exception("'File' does not exist");
            }

            return new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(contents)));
        }
    }
}
