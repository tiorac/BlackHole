using BlackHole.Shared;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace BlackHole.Server.Hubs
{
    public static class SenderQueueService
    {
        public static async Task QueueAdd<T>(this IHubContext<ServiceQueueHub> hub, T serviceData)
        {
            QueueMessage queueMessage = new()
            {
                MethodName = nameof(QueueAdd),
                Value = JsonSerializer.Serialize(serviceData),
                ValueType = typeof(T).AssemblyQualifiedName,
            };

            await hub.Clients.All.SendAsync(nameof(QueueAdd), queueMessage);
        }

        public static async Task QueueUpdated(this IHubContext<ServiceQueueHub> hub, ServiceData serviceData)
        {
            QueueMessage queueMessage = new()
            {
                MethodName = nameof(QueueUpdated),
                Value = JsonSerializer.Serialize(serviceData),
                ValueType = serviceData.GetType().AssemblyQualifiedName,
            };

            await hub.Clients.All.SendAsync(nameof(QueueUpdated), queueMessage);
        }

        public static async Task QueueRemoved(this IHubContext<ServiceQueueHub> hub, Guid id)
        {
            QueueMessage queueMessage = new()
            {
                MethodName = nameof(QueueRemoved),
                Value = JsonSerializer.Serialize(id),
                ValueType = typeof(Guid).AssemblyQualifiedName,
            };

            await hub.Clients.All.SendAsync(nameof(QueueRemoved), queueMessage);
        }
    }
}
