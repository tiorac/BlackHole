using BlackHole.Server.Services.Downloads;
using System.Collections.ObjectModel;

namespace BlackHole.Server.Services
{
    public class ServiceControl
    {
        public ServiceControl(IConfiguration configuration)
        {
            Services = [];
            Queue = [];
            _configuration = configuration;

            CreateDefaultServices();
        }

        private readonly IConfiguration _configuration;

        private List<IService> Services { get; set; }

        public List<ServiceData> Queue { get; private set; }

        public ReadOnlyCollection<ServiceData> Waiting =>
            Queue.Where(s => s.Progress == null && !s.Error)
                 .ToList()
                 .AsReadOnly();

        public ReadOnlyCollection<ServiceData> Processing =>
            Queue.Where(s => !s.Error 
                            && s.Progress.HasValue 
                            && s.Progress.Value > 0
                            && !s.IsCompleted)
                 .ToList()
                 .AsReadOnly();

        public ReadOnlyCollection<ServiceData> Processed =>
            Queue.Where(s => s.IsCompleted || s.Error)
                 .ToList()
                 .AsReadOnly();

        private void CreateDefaultServices()
        {
            Services.Add(new Downloader(_configuration));
        }


        public IEnumerable<T> GetQueueByType<T>() 
            where T : ServiceData
        => Queue.Where(s => s is T)
                .Cast<T>();

        public void RegisterService(IService service)
        {
            Services.Add(service);
        }

        public void UnregisterService(IService service)
        {
            Services.Remove(service);
        }

        public IService? GetService(ServiceData serviceData)
        {
            foreach (var service in Services)
            {
                if (service.IsDataValid(serviceData))
                    return service;
            }

            return null;
        }

        public ServiceData? Get(Guid id)
        {
            return Queue.FirstOrDefault(s => s.Id == id);
        }

        public T? Get<T>(Guid id) 
            where T : ServiceData
        {
            return Queue.FirstOrDefault(s => s.Id == id && s is T) as T;
        }

        public bool Add(ServiceData service)
        {
            if (Queue.Contains(service))
                return false;

            Queue.Add(service);
            return true;
        }

        public bool Remove(Guid id)
        {
            var service = Queue.FirstOrDefault(s => s.Id == id);

            if (service != null)
                return Queue.Remove(service);

            return false;
        }

        public bool Remove(ServiceData service)
        {
            if (!service.IsProcessing
                && Queue.Contains(service))
            {
                Queue.Remove(service);
                return true;
            }

            return false;
        }
    }
}
