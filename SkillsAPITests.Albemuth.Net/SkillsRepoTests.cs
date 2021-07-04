using SkillsAPI.Albemuth.Net.Services;
using SkillsAPITests.Albemuth.Net.Generators;
using SkillsAPITests.Albemuth.Net.Mocks;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SkillsAPITests.Albemuth.Net
{
    public class SkillsRepoTests
    {
        private readonly ITestOutputHelper output;

        public SkillsRepoTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private (MemoryFileIO, SkillsRepo) BuildTestServices()
        {
            var memoryFileIO = new MemoryFileIO();
            var yamlThang = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            var repo = new SkillsRepo(
                output.BuildLoggerFor<SkillsRepo>(),
                new MockAppSettings
                {
                    DataFolder = Const.TestDataFolder
                },
                memoryFileIO,
                new SkillsFileReader(output.BuildLoggerFor<SkillsFileReader>(), memoryFileIO),
                yamlThang);

            return (memoryFileIO, repo);
        }

        [Fact]
        public async void TestSkillSummaries()
        {
            var (memoryFileIO, skillsRepo) = BuildTestServices();
            MemoryFileIOFileSystemGenerator.AddTestFileSystem1(memoryFileIO, Const.TestDataFolder);

            var summaries = await skillsRepo.GetSkillSummaries();

            Assert.Collection(summaries,
                item =>
                {
                    Assert.Equal("TIPP", item.ID);
                    Assert.Equal("DBT", item.Area);
                    Assert.Equal("Distress Tolerance", item.Section);
                    Assert.Equal("TIPP", item.Title);
                },
                item =>
                {
                    Assert.Equal("UnrelentingStandards", item.ID);
                    Assert.Equal("Schema Therapy", item.Area);
                    Assert.Equal("Schemas", item.Section);
                    Assert.Equal("Unrelenting standards", item.Title);
                }
            );
        }

        [Fact]
        public async void TestSkillDetail()
        {
            var (memoryFileIO, skillsRepo) = BuildTestServices();
            MemoryFileIOFileSystemGenerator.AddTestFileSystem1(memoryFileIO, Const.TestDataFolder);

            var item = await skillsRepo.GetSkillByID("TIPP");

            Assert.Equal("TIPP", item.ID);
            Assert.Equal("DBT", item.Area);
            Assert.Equal("Distress Tolerance", item.Section);
            Assert.Equal("TIPP", item.Title);
        }
    }
}
