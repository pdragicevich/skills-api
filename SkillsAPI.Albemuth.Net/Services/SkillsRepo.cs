using Microsoft.Extensions.Logging;
using SkillsAPI.Albemuth.Net.Contracts;
using SkillsAPI.Albemuth.Net.Extensions;
using SkillsAPI.Albemuth.Net.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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

        public SkillsRepo(
            ILogger<SkillsRepo> logger,
            IAppSettings appSettings,
            IFileIO fileIO,
            IDeserializer yamlDeserialiser)
        {
            this.logger = logger;
            this.appSettings = appSettings;
            this.fileIO = fileIO;
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


        private async Task<MarkdownContents?> ReadFile(string path, bool frontMatterOnly)
        {
            if (!fileIO.FileExists(path))
            {
                return null;
            }

            StreamReader? reader = null;
            try
            {
                reader = fileIO.OpenFileStreamReader(path);

                var frontMatterBuilder = new StringBuilder();
                var contentsBuilder = frontMatterOnly ? null : new StringBuilder();
                var inFrontMatter = false;
                var hasFrontMatter = false;

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (line.GuardedTrim() == Const.FrontMatterDelimiter)
                    {
                        inFrontMatter = !inFrontMatter;
                    }
                    else if (inFrontMatter)
                    {
                        hasFrontMatter = true;
                        frontMatterBuilder.AppendLine(line);
                    }
                    else if (frontMatterOnly && hasFrontMatter)
                    {
                        break;
                    }
                    else
                    {
                        contentsBuilder?.AppendLine(line);
                    }
                }

                if (!hasFrontMatter)
                {
                    return null;
                }


                return new MarkdownContents
                {
                    FrontMatter = frontMatterBuilder.ToString(),
                    Contents = contentsBuilder?.ToString()
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ReadFile EXCEPTION");
            }
            finally
            {
                reader?.Dispose();
            }
            return null;
        }



        private async Task<SkillSummary?> ReadFileSummary(string path)
        {
            var fileContents = await ReadFile(path, frontMatterOnly: true);
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
            var fileContents = await ReadFile(path, frontMatterOnly: false);
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
