namespace BlackHole.Server.Services.Downloads
{
    public class Downloader : IService
    {
        public Downloader(IConfiguration configuration, ServiceRepository serviceRepository)
        {
            _downloads = new();
            _configuration = configuration;
            _serviceRepository = serviceRepository;
            CreateDefaultDownloads();
        }

        private List<IDownloader> _downloads;
        private readonly IConfiguration _configuration;
        private readonly ServiceRepository _serviceRepository;

        private void CreateDefaultDownloads()
        {
            _downloads.Add(new YoutubeDownloader(_configuration));
            _downloads.Add(new FileDownloader(_configuration));
        }
        
        public void Add(IDownloader downloader)
        {
            _downloads.Insert(0, downloader);
        }

        public IDownloader? Find(string url)
        {
            return _downloads.FirstOrDefault(d => d.IsValid(url));
        }

        public bool IsDataValid(ServiceData data)
        {
            return data is DownloadData;
        }

        public async Task RunAsync(ServiceData data, Action<ServiceData> updated, CancellationToken cancellationToken)
        {
            DownloadData downloadData = (DownloadData)data;

            var downloader = Find(downloadData.OriginUrl);

            if (downloader == null)
            {
                downloadData.Error = true;
                downloadData.ErrorMessage = "No downloader found for this url.";
                return;
            }

            try
            {
                await downloader.PrepareAsync(downloadData, updated, cancellationToken);
                await downloader.DownloadAsync(downloadData, updated, cancellationToken);
            }
            catch (Exception ex)
            {
                downloadData.Error = true;
                downloadData.ErrorMessage = ex.Message;
                updated(downloadData);
            }

            _serviceRepository.Update(downloadData);
        }

        private string GetDestinationFileName(string originUrl)
        {
            string fileName = Path.GetFileName(originUrl);

            return Path.Join("D:\\Teste\\temp", fileName);
        }
    }
}
