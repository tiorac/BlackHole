using BlackHole.Client.Services;
using Microsoft.AspNetCore.Components;

namespace BlackHole.Client
{
    public partial class App
    {
        [Inject]
        public QueueService QueueService { get; set; }

        override protected void OnInitialized()
        {
            base.OnInitialized();
            QueueService.Start();
        }

        public async ValueTask DisposeAsync()
        {
            QueueService.Dispose();
        }
    }
}
