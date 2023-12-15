using BlackHole.Server.Services;
using BlackHole.Shared;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace BlackHole.Server.Hubs
{
    public class ServiceQueueHub : Hub
    {
        public ServiceQueueHub(ServiceControl serviceControl)
        {
            _serviceControl = serviceControl;
        }

        private readonly ServiceControl _serviceControl;

        override public async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            QueueMessage queueMessage = new()
            {
                MethodName = nameof(QueueMessage),
                Value = JsonSerializer.Serialize(_serviceControl.Queue),
                ValueType = _serviceControl.Queue.GetType().AssemblyQualifiedName,
            };

            await Clients.Client(Context.ConnectionId).SendAsync("QueueList", queueMessage);
        }
    }
}
