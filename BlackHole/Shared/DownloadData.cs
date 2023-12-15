
using System.Text.Json.Serialization;

namespace BlackHole
{
    public class DownloadData : ServiceData
    {
        public string OriginUrl { get; set; }

        public string? PathDestination { get; set; }

        public string? FileName { get; set; }

        [JsonIgnore]
        public string Extension
        {
            get => Path.GetExtension(FileName ?? OriginUrl);
        }

        [JsonIgnore]
        public string FileNameWithoutExtension
        {
            get => Path.GetFileNameWithoutExtension(FileName ?? OriginUrl);
        }

        public override string ToString()
        {
            return OriginUrl;
        }

        public override int GetHashCode()
        {
            return OriginUrl.GetHashCode();
        }

        override public bool Equals(object? obj)
        {
            if (obj is DownloadData data)
                return OriginUrl == data.OriginUrl;

            return false;
        }
    }
}
