using SkillsAPI.Albemuth.Net.Models;
using System.Threading.Tasks;

namespace SkillsAPI.Albemuth.Net.Contracts
{
    public interface ISkillsFileReader
    {
        Task<MarkdownContents?> ReadFile(string path, bool frontMatterOnly);
    }
}
