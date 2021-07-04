using System.Collections;
using System.Collections.Generic;

namespace SkillsAPITests.Albemuth.Net.Generators
{
    internal class SkillsFileReaderTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new()
        {
            new object[] { "EmptyFile.md", "", false },
            new object[] { "NoFrontMatter.md", "some markdown\nhere\nyay", false },
            new object[] { "BadFrontMatterStart.md", "some markdown\nhere\nyay\n---\nfront matter\n---\n", false },
            new object[] { "FrontMatterDoesntEnd.md", "---\nfront matter\nother stuff\nanother line\n", false },
            new object[] { "ValidFile.md", "---\nfront matter\n---\ncontents\n", true, "front matter", "contents" }
        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
