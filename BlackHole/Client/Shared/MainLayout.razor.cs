namespace BlackHole.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        private string BrowserTitle { get; set; } = "BlackHole";
        private string Title { get; set; } = "";

        override protected void OnInitialized()
        {
            PageTitleService.OnTitleChanged += OnTitleChanged;
        }

        private void OnTitleChanged(string title)
        {
            Title = title;
            BrowserTitle = $"{title} - BlackHole";
            StateHasChanged();
        }

        public void Dispose()
        {
            PageTitleService.OnTitleChanged -= OnTitleChanged;
        }
    }
}
