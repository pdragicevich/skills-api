using SkillsAPI.Albemuth.Net.Extensions;
using SkillsAPI.Albemuth.Net.Services;
using SkillsAPITests.Albemuth.Net.Generators;
using SkillsAPITests.Albemuth.Net.Mocks;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SkillsAPITests.Albemuth.Net
{
    public class SkillsFileReaderTests
    {
        private readonly ITestOutputHelper output;

        public SkillsFileReaderTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Theory]
        [ClassData(typeof(SkillsFileReaderTestDataGenerator))]
        public async Task ReadFileTest(
            string filename,
            string contents,
            bool expectValue,
            string expectedFrontMatter = null,
            string expectedContents = null)
        {
            var mockFileIO = new MemoryFileIO();
            mockFileIO.FileContents.Add(filename, contents);
            var reader = new SkillsFileReader(output.BuildLoggerFor<SkillsFileReader>(), mockFileIO);
            var result = await reader.ReadFile(filename, frontMatterOnly: false);
            if (expectValue)
            {
                Assert.NotNull(result);
                Assert.Equal(expectedFrontMatter, result.FrontMatter.GuardedTrim());
                Assert.Equal(expectedContents, result.Contents.GuardedTrim());
            }
            else
            {
                Assert.Null(result);
            }
        }
    }
}
