namespace BlackHole.Server.Services.Downloads
{
    public interface IDownloader
    {
        bool IsValid(string url);

        Task PrepareAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken);

        Task DownloadAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken);
    }
}
