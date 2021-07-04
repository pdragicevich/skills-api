using Microsoft.Extensions.Logging;
using SkillsAPI.Albemuth.Net.Contracts;
using SkillsAPI.Albemuth.Net.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace SkillsAPI.Albemuth.Net.Services
{
    public class SkillsRepo : ISkillsRepo
    {
        private readonly ILogger<SkillsRepo> logger;
        private readonly IAppSettings appSettings;
        private readonly IDeserializer yamlDeserialiser;
        private readonly IFileIO fileIO;
        private readonly ISkillsFileReader skillsFileReader;

        public SkillsRepo(
            ILogger<SkillsRepo> logger,
            IAppSettings appSettings,
            IFileIO fileIO,
            ISkillsFileReader skillsFileReader,
            IDeserializer yamlDeserialiser)
        {
            this.logger = logger;
            this.appSettings = appSettings;
            this.fileIO = fileIO;
            this.skillsFileReader = skillsFileReader;
            this.yamlDeserialiser = yamlDeserialiser;
        }

        public async Task<IList<SkillSummary>> GetSkillSummaries()
        {
            var dirListing = await GetSkillsFiles();
            var skills = new List<SkillSummary>(Const.SummaryListSize);
            foreach (var file in dirListing)
            {
                var summary = await ReadFileSummary(file);
                if (summary != null)
                {
                    skills.Add(summary);
                }
            }
            return skills;
        }

        public async Task<SkillDetail?> GetSkillByID(string id)
        {
            var path = BuildFilePath(id);
            var detail = await ReadFileDetail(path);
            return detail;
        }

        private Task<IEnumerable<string>> GetSkillsFiles()
        {
            return Task.Run(() =>
            {
                return fileIO.GetAllFiles();
            });
        }

        private string BuildFilePath(string id)
        {
            return Path.Combine(appSettings.DataFolder, Path.ChangeExtension(id, Const.MarkdownExtension));
        }

        private static string ParseIdFromFilePath(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        private async Task<SkillSummary?> ReadFileSummary(string path)
        {
            var fileContents = await skillsFileReader.ReadFile(path, frontMatterOnly: true);
            if (fileContents == null || string.IsNullOrWhiteSpace(fileContents.FrontMatter))
            {
                return null;
            }

            var deserialised = yamlDeserialiser.Deserialize<SkillSummary>(fileContents.FrontMatter);
            deserialised.ID = ParseIdFromFilePath(path);

            return deserialised;
        }

        private async Task<SkillDetail?> ReadFileDetail(string path)
        {
            var fileContents = await skillsFileReader.ReadFile(path, frontMatterOnly: false);
            if (fileContents == null || string.IsNullOrWhiteSpace(fileContents.FrontMatter))
            {
                return null;
            }

            var deserialised = yamlDeserialiser.Deserialize<SkillDetail>(fileContents.FrontMatter);
            deserialised.ID = ParseIdFromFilePath(path);
            deserialised.Content = fileContents.Contents;

            return deserialised;
        }
    }
}
