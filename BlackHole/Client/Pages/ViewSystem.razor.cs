using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace BlackHole.Client.Pages
{
    public partial class ViewSystem
    {
        [Inject]
        public HttpClient Client { get; set; }

        [Inject]
        public ErrorHandler ErrorHandler { get; set; }

        public Dictionary<string, string> Config { get; set; } = new Dictionary<string, string>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            try
            {
                var config = await Client.GetFromJsonAsync<Dictionary<string, string>>("api/config/system");

                if (config != null)
                {
                    Config = config;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.AddError(ex);
            }
        }
    }
}
