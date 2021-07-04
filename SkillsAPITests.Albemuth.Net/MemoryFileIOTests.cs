using SkillsAPITests.Albemuth.Net.Generators;
using SkillsAPITests.Albemuth.Net.Mocks;
using Xunit;

namespace SkillsAPITests.Albemuth.Net
{
    public class MemoryFileIOTests
    {
        [Fact]
        public void TestDirectoryEnumeration()
        {
            var io = new MemoryFileIO();
            MemoryFileIOFileSystemGenerator.AddTestFileSystem1(io, Const.TestDataFolder);

            var files = io.GetAllFiles();

            Assert.Collection(files,
                item => Assert.Equal($"{Const.TestDataFolder}\\TIPP.md", item),
                item => Assert.Equal($"{Const.TestDataFolder}\\UnrelentingStandards.md", item));
        }
    }
}
