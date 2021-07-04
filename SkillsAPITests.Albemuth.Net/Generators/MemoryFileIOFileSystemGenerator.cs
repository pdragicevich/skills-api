using SkillsAPITests.Albemuth.Net.Mocks;

namespace SkillsAPITests.Albemuth.Net.Generators
{
    internal static class MemoryFileIOFileSystemGenerator
    {
        public static void AddTestFileSystem1(MemoryFileIO memoryFileIO, string folder)
        {
            memoryFileIO.FileContents.Add($"{folder}\\TIPP.md",
@"---
area: DBT
section: Distress Tolerance
title: TIPP
summary: Temperature, intense exercise, paired muscle relaxation,
---
Temperature, intense exercise, paired muscle relaxation
"
                );
            memoryFileIO.FileContents.Add($"{folder}\\UnrelentingStandards.md",
@"---
area: Schema Therapy
section: Schemas
title: Unrelenting standards
summary: Unrelenting standards
---
Unrelenting standards
"
                );
        }
    }
}
