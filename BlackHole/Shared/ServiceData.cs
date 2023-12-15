using System.Text.Json.Serialization;

namespace BlackHole
{
    [JsonDerivedType(typeof(DownloadData), "DownloadData")]
    public abstract class ServiceData
    {
        public ServiceData()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int? Progress { get; set; }

        public bool Error { get; set; }

        public string? ErrorMessage { get; set; }

        [JsonIgnore]
        public bool IsCompleted
        {
            get => Progress == 100;
        }

        [JsonIgnore]
        public bool IsProcessing
        {
            get => Progress.HasValue
                && !IsCompleted
                && !Error;
        }
    }
}
