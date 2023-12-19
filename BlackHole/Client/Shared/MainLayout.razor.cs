using Microsoft.AspNetCore.Components;

namespace BlackHole.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        private string BrowserTitle { get; set; } = "BlackHole";
        private string Title { get; set; } = "";
        private RenderFragment? ToolsButtons { get; set; }

        override protected void OnInitialized()
        {
            PageTitleService.MainPageChanged += MainPageChanged;
        }

        private void MainPageChanged()
        {
            Title = PageTitleService.Title;
            BrowserTitle = $"{Title} - BlackHole";
            ToolsButtons = PageTitleService.ToolsButtons;
            StateHasChanged();
        }

        public void Dispose()
        {
            PageTitleService.MainPageChanged -= MainPageChanged;
        }
    }
}
