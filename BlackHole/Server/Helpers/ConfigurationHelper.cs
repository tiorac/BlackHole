
namespace BlackHole.Server.Helpers
{
    public static class ConfigurationHelper
    {
        public static T? FindAndGetValue<T>(this IConfiguration configuration, string key, T defaultValue)
        {
            string envKey = $"BLACKHOLE_{key.ToUpper().Replace(':', '_')}";

            object? value = null;
            value = Environment.GetEnvironmentVariable(envKey);

            if (value is not null)
            {
                return Convert.ChangeType(value, typeof(T)) is T convertedValue
                    ? convertedValue
                    : defaultValue;
            }

            return configuration.GetValue(key, defaultValue);
        }

        public static string GetConfigFolder()
        {
            string? path = Environment.GetEnvironmentVariable("BLACKHOLE_CONFIG");

            if (string.IsNullOrWhiteSpace(path))
            {
                path = Directory.GetCurrentDirectory();
                path = Path.Combine(path, "config");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }

            return path;
        }
    }
}
