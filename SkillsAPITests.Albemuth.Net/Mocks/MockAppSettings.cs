using SkillsAPI.Albemuth.Net.Contracts;

namespace SkillsAPITests.Albemuth.Net.Mocks
{
    internal class MockAppSettings : IAppSettings
    {
        public string DataFolder { get; set; }
    }
}
