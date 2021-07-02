using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkillsAPI.Albemuth.Net.Contracts;
using SkillsAPI.Albemuth.Net.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SkillsAPI.Albemuth.Net.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SkillsController : ControllerBase
    {
        private readonly ILogger<SkillsController> logger;
        private readonly ISkillsRepo skillsRepo;

        public SkillsController(
            ILogger<SkillsController> logger,
            ISkillsRepo skillsRepo
            )
        {
            this.logger = logger;
            this.skillsRepo = skillsRepo;
        }

        [HttpGet]
        public async Task<IEnumerable<SkillSummary>> Get()
        {
            return await skillsRepo.GetSkillSummaries();
        }






    }
}
