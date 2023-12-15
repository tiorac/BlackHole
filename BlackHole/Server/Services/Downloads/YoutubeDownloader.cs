

using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace BlackHole.Server.Services.Downloads
{
    public class YoutubeDownloader : IDownloader
    {
        public YoutubeDownloader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public bool IsValid(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return url.Contains("youtube.com") || url.Contains("youtu.be");
        }

        public async Task PrepareAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken)
        {
            if (downloadData == null)
                throw new ArgumentNullException(nameof(downloadData));


            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(downloadData.OriginUrl, cancellationToken);
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(downloadData.OriginUrl, cancellationToken);
            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            string title = video.Title.Replace("\n", "")
                                      .Replace("\r", "")
                                      .Replace("\t", "_")
                                      .Replace(" ", "_")
                                      .Replace(":", "")
                                      .Replace("/", "")
                                      .Replace("\\", "")
                                      .Replace("*", "")
                                      .Replace("?", "")
                                      .Replace("\"", "")
                                      .Replace("<", "")
                                      .Replace(">", "")
                                      .Replace("|", "");

            title = $"{title}.{streamInfo.Container.Name}";

            downloadData.FileName = title;
            downloadData.PathDestination = Path.Combine(_configuration.DownloadsPath(), downloadData.FileName);
            downloadData.Progress = 0;

            CallUpdated(downloadData, updated);
        }

        /// <summary>
        /// Download youtube video from URL
        /// </summary>
        /// <param name="oringUrl"></param>
        /// <param name="pathDestination"></param>
        /// <param name="progress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task DownloadAsync(DownloadData downloadData, Action<ServiceData>? updated, CancellationToken cancellationToken)
        {
            if (downloadData == null)
                throw new ArgumentNullException(nameof(downloadData));


            var youtube = new YoutubeClient();
            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(downloadData.OriginUrl, cancellationToken);

            var streamInfo = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
            var stream = await youtube.Videos.Streams.GetAsync(streamInfo, cancellationToken);

            Progress<double>? progess = null;

            if (updated != null)
            {
                progess = new(p =>
                {
                    int currentProgress = (int)Math.Truncate(p * 100);

                    if (currentProgress - downloadData.Progress > 1)
                    {
                        downloadData.Progress = currentProgress;
                        CallUpdated(downloadData, updated);
                    }
                });
            }

            await youtube.Videos.Streams.DownloadAsync(streamInfo, downloadData.PathDestination, progess, cancellationToken);
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
