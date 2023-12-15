namespace BlackHole.Server.Services
{
    public interface IService
    {
        bool IsDataValid(ServiceData data);

        Task RunAsync(ServiceData data, Action<ServiceData> updated, CancellationToken cancellationToken);
    }
}
