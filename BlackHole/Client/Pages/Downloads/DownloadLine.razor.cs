using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;

namespace BlackHole.Client.Pages.Downloads
{
    public partial class DownloadLine
    {
        [Inject]
        public HttpClient Client { get; set; }

        [Inject]
        public ErrorHandler ErrorHandler { get; set; }

        [Parameter]
        public DownloadData Download { get; set; }

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
    }
}
