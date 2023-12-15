
using BlackHole.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BlackHole.Server.Services
{
    public class ServiceWorker : BackgroundService
    {
        public ServiceWorker(ServiceControl serviceControl, IHubContext<ServiceQueueHub> queueService)
        {
            _serviceControl = serviceControl;
            _queueService = queueService;
        }

        private const int limit = 1;
        private readonly ServiceControl _serviceControl;
        private readonly IHubContext<ServiceQueueHub> _queueService;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_serviceControl.Processing.Count < limit
                    && _serviceControl.Waiting.Count >= 1)
                {
                    var serviceData = _serviceControl.Waiting.First();
                    var service = _serviceControl.GetService(serviceData);

                    if (service is not null)
                    {
                        await service.RunAsync(serviceData, UpdatedServiceData, stoppingToken);
                    }
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }

        private async void UpdatedServiceData(ServiceData serviceData)
        {
            await _queueService.QueueUpdated(serviceData);
        }
    }
}
