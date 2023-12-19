using System.Net.Http.Json;
using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;


namespace BlackHole.Client.Pages.Downloads
{
    public partial class DownloadPage : IDisposable
    {
        [Inject]
        public HttpClient Client { get; set; }

        [Inject]
        public ErrorHandler ErrorHandler { get; set; }

        [Inject]
        public QueueService QueueService { get; set; }

        [Inject]
        public MainPageService PageTitleService { get; set; }

        public List<DownloadData> Downloads
            => QueueService.GetServices<DownloadData>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            QueueService.OnChange += UpdateLayout;
        }

        private void UpdateLayout()
        {
            InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private bool CanAddUrl(string url)
        {
            if (url == null)
                return false;

            if (url.Contains('\n'))
                return url.Replace("\r", "").Split("\n").All(CanAddUrl);

            return !Downloads.Any(x => x.OriginUrl == url);
        }

        public async void UrlToDownload(string url)
        {
            try
            {
                if (url == null)
                    return;

                if (url.Contains('\n'))
                {
                    var urls = url.Replace("\r", "").Split("\n");
                    foreach (var u in urls)
                    {
                        UrlToDownload(u);
                    }

                    return;
                }

                var download = new DownloadData
                {
                    OriginUrl = url
                };

                await Client.PostAsJsonAsync("api/download", download);
            }
            catch (Exception ex)
            {
                ErrorHandler.AddError(ex);
            }
        }

        public void Dispose()
        {
            PageTitleService.ToolsButtons = null;
            QueueService.OnChange -= UpdateLayout;
        }
    }
}
