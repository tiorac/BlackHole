using BlackHole.Shared;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Text.Json;

namespace BlackHole.Client.Services
{
    public class QueueService : IDisposable
    {
        public QueueService(HubConnection hubConnection)
        {
            HubConnection = hubConnection;
            CancelToken = new();
            Services = new();
        }

        private List<ServiceData> Services { get; set; }
        private HubConnection HubConnection { get; set; }
        private CancellationTokenSource CancelToken { get; set; }

        public event Action OnChange;

        public async void Start()
        {
            HubConnection.Closed += SignalErrorAsync;

            QueueAdd();
            QueueList();
            QueueUpdated();
            QueueRemoved();

            await HubConnection.StartAsync();
            //await ConnectWithRetryAsync(CancelToken.Token);
        }

        private async Task<bool> ConnectWithRetryAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    await HubConnection.StartAsync(token);
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    await Task.Delay(5000);
                }
            }
        }

        private Task SignalErrorAsync(Exception? ex)
        {
            return ConnectWithRetryAsync(CancelToken.Token);
        }

        private void QueueList()
        {
            HubConnection.On<QueueMessage>(nameof(QueueList), (queueMessage) =>
            {
                var data = JsonSerializer.Deserialize(queueMessage.Value, Type.GetType(queueMessage.ValueType));

                if (data is List<ServiceData> services)
                    Services = services;

                OnChange?.Invoke();
            });
        }

        private void QueueAdd()
        {
            HubConnection.On<QueueMessage>(nameof(QueueAdd), (queueMessage) =>
            {
                var data = JsonSerializer.Deserialize(queueMessage.Value, Type.GetType(queueMessage.ValueType));

                if (data is ServiceData service)
                    AddOrUpdateService(service);

                OnChange?.Invoke();
            });
        }

        private void QueueUpdated()
        {
            HubConnection.On<QueueMessage>(nameof(QueueUpdated), (queueMessage) =>
            {
                var data = JsonSerializer.Deserialize(queueMessage.Value, Type.GetType(queueMessage.ValueType));

                if (data is ServiceData service)
                    AddOrUpdateService(service);

                OnChange?.Invoke();
            });
        }

        private void QueueRemoved()
        {
            HubConnection.On<QueueMessage>(nameof(QueueRemoved), (queueMessage) =>
            {
                var data = JsonSerializer.Deserialize(queueMessage.Value, Type.GetType(queueMessage.ValueType));

                if (data is Guid id)
                    RemoveService(id);

                OnChange?.Invoke();
            });
        }


        public List<T> GetServices<T>() 
            where T : ServiceData
        {
            return Services.OfType<T>().ToList();
        }

        private void AddOrUpdateService(ServiceData service)
        {
            var index = Services.FindIndex(s => s.Id == service.Id);

            if (index == -1)
                Services.Add(service);
            else
                Services[index] = service;
        }

        private void RemoveService(Guid id)
        {
            var index = Services.FindIndex(s => s.Id == id);

            if (index != -1)
            {
                Services.RemoveAt(index);
                OnChange?.Invoke();
            }
        }

        public async void Dispose()
        {
            CancelToken.Cancel();
            CancelToken.Dispose();
            await HubConnection.DisposeAsync();
        }
    }
}
