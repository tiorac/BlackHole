namespace BlackHole.Client.Pages
{
    public partial class Index : IDisposable
    {
        override protected void OnInitialized()
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

        public void Dispose()
        {
            QueueService.OnChange -= UpdateLayout;
        }
    }
}
