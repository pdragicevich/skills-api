using SkillsAPI.Albemuth.Net.Contracts;
using SkillsAPI.Albemuth.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace SkillsAPI.Albemuth.Net.Services
{
    public class SkillsRepo : ISkillsRepo
    {
        public Task<IList<SkillSummary>> GetSkillSummaries()
        {
            return Task.Run<IList<SkillSummary>>(() =>
            {

                return Enumerable.Empty<SkillSummary>().ToList();
            });
        }


        public Task<SkillDetail> GetSkillByID(string id)
        {
            return Task.Run<SkillDetail>(() =>
            {

                return new SkillDetail();
            });

        }

    }
}
