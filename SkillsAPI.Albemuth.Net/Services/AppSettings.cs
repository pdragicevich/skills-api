using Microsoft.Extensions.Configuration;
using SkillsAPI.Albemuth.Net.Contracts;

namespace SkillsAPI.Albemuth.Net.Services
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;

        public AppSettings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public string DataFolder => GetSetting("AppSettings:DataFolder", ".\\Data");


        private T GetSetting<T>(string key, T defaultValue)
        {
            return configuration.GetValue<T>(key) ?? defaultValue;
        }
    }
}
