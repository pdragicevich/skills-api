using Microsoft.Extensions.Logging;
using SkillsAPI.Albemuth.Net.Contracts;
using SkillsAPI.Albemuth.Net.Extensions;
using SkillsAPI.Albemuth.Net.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SkillsAPI.Albemuth.Net.Services
{
    public class SkillsFileReader : ISkillsFileReader
    {
        private readonly ILogger<SkillsFileReader> logger;
        private readonly IFileIO fileIO;

        public SkillsFileReader(
            ILogger<SkillsFileReader> logger,
            IFileIO fileIO)
        {
            this.logger = logger;
            this.fileIO = fileIO;
        }

        public async Task<MarkdownContents?> ReadFile(string path, bool frontMatterOnly)
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
                var firstLine = true;

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

                    if (firstLine && !inFrontMatter)
                    {
                        break;
                    }
                    firstLine = false;
                }

                if (!hasFrontMatter || inFrontMatter)
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
    }
}
