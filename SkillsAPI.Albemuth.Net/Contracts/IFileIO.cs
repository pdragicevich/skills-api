using System.Collections.Generic;
using System.IO;

namespace SkillsAPI.Albemuth.Net.Contracts
{
    public interface IFileIO
    {
        IEnumerable<string> GetAllFiles();

        bool FileExists(string path);

        StreamReader OpenFileStreamReader(string path);
    }
}
