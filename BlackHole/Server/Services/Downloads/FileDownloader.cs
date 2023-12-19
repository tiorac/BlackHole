using BlackHole.Server.Helpers;

namespace BlackHole.Server.Services.Downloads
{
    public class FileDownloader : IDownloader
    {
        public FileDownloader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public bool IsValid(string url) => !string.IsNullOrWhiteSpace(url);

        public Task PrepareAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken)
        {
            downloadData.FileName = Path.GetFileName(downloadData.OriginUrl);
            downloadData.PathDestination = Path.Combine(_configuration.DownloadsPath(), downloadData.FileName);
            downloadData.Progress = 0;

            CallUpdated(downloadData, updated);

            return Task.CompletedTask;
        }

        public async Task DownloadAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken)
        {
            using HttpClient client = new();
            using HttpResponseMessage response = await client.GetAsync(downloadData.OriginUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                downloadData.Error = true;
                int statusCode = (int)response.StatusCode;

                downloadData.ErrorMessage = $"{statusCode}: {response.StatusCode}";
                CallUpdated(downloadData, updated);
                return;
            }

            var contentLength = response.Content.Headers.ContentLength;

            using Stream downloadStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            await using FileStream fileStream = new(downloadData.PathDestination, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);

            Progress<long>? progress = null;

            if (updated != null && contentLength.HasValue)
            {
                progress = new Progress<long>(p =>
                {
                    int currentProgress = (int)((double)p / contentLength.Value * 100);
                    
                    if (currentProgress - downloadData.Progress > 1)
                    {
                        downloadData.Progress = currentProgress;
                        fileStream.Flush();

                        CallUpdated(downloadData, updated);
                    }
                });
            }

            await downloadStream.CopyToAsync(fileStream, 81920, progress, cancellationToken);

            downloadData.Progress = 100;
            CallUpdated(downloadData, updated);
        }

        private static void CallUpdated(DownloadData downloadData, Action<ServiceData>? updated)
        {
            try
            {
                updated?.Invoke(downloadData);
            }
            catch (Exception)
            {
            }
        }
    }
}
