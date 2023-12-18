using BlackHole.Server.Helpers;
using LiteDB;

namespace BlackHole.Server
{
    public abstract class BaseRepository
    {
        protected static object LockLiteDB = new();

        public static string GetFileName()
        {
            var path = ConfigurationHelper.GetConfigFolder();
            path = Path.Combine(path, "data");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fullPath = Path.Combine(path, "blackhole.db");
            return fullPath;
        }

        protected string GetConnectionString()
        {
            var fullPath = GetFileName();
            var connectionString = $"Filename={fullPath}";

            return connectionString;
        }

        protected LiteDatabase CreateDatabase() => new(GetConnectionString());
    }
}
