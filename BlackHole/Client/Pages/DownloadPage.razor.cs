using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlackHole.Client.Pages
{
    public partial class DownloadPage : IDisposable
    {
        [Inject]
        public HttpClient Client { get; set; }

        [Inject]
        public ErrorHandler ErrorHandler { get; set; }

        [Inject]
        public QueueService QueueService { get; set; }

        public List<DownloadData> Downloads 
            => QueueService.GetServices<DownloadData>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            QueueService.OnChange += UpdateLayout;
        }

        private void UpdateLayout()
        {
            this.InvokeAsync(() =>
            {
                StateHasChanged();
            });
        }

        private bool CanAddUrl(string url)
        {
            return !Downloads.Any(x => x.OriginUrl == url);
        }

        public async void UrlToDownload(string url)
        {
            try
            {
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

        public async void DeleteDownload(Guid id)
        {
            try
            {
                await Client.DeleteAsync($"api/download/{id}");
            }
            catch (Exception ex)
            {
                ErrorHandler.AddError(ex);
            }
        }

        public void Dispose()
        {
            QueueService.OnChange -= UpdateLayout;
        }
    }
}
