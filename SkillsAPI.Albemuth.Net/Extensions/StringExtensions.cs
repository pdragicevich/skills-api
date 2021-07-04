namespace SkillsAPI.Albemuth.Net.Extensions
{
    public static class StringExtensions
    {
        public static string GuardedTrim(this string? str)
        {
            return (str ?? string.Empty).Trim();
        }
    }
}
