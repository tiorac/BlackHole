using BlackHole.Server.Helpers;

namespace BlackHole.Server.Services.Downloads
{
    public static class DownloadsConfig
    {
        public static string DownloadsPath(this IConfiguration configuration)
            => configuration.FindAndGetValue("Downloads:Path", "Downloads")!;
    }
}
