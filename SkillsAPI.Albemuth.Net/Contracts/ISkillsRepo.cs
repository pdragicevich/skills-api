using SkillsAPI.Albemuth.Net.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsAPI.Albemuth.Net.Contracts
{
    public interface ISkillsRepo
    {
        Task<IList<SkillSummary>> GetSkillSummaries();
        Task<SkillDetail?> GetSkillByID(string id);
    }
}
