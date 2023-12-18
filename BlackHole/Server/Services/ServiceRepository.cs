
namespace BlackHole.Server.Services
{
    public class ServiceRepository(ErrorHandler errorHandler) : BaseRepository()
    {
        private const string CollectionName = "Services";
        private readonly ErrorHandler _errorHandler = errorHandler;

        public List<ServiceData> GetAll()
        {
            lock (LockLiteDB)
            {
                try
                {
                    using var db = CreateDatabase();
                    var collection = db.GetCollection<ServiceData>(CollectionName);

                    return collection.FindAll().ToList();
                }
                catch (Exception ex)
                {
                    _errorHandler.AddError(ex);
                    return [];
                }
            }
        }

        public ServiceData Get(Guid id)
        {
            lock (LockLiteDB)
            {
                try
                {
                    using var db = CreateDatabase();
                    var collection = db.GetCollection<ServiceData>(CollectionName);

                    return collection.FindOne(s => s.Id == id);
                }
                catch (Exception ex)
                {
                    _errorHandler.AddError(ex);
                    return null;
                }                
            }
        }

        public void Update(ServiceData service)
        {
            lock (LockLiteDB)
            {
                try
                {
                    using var db = CreateDatabase();
                    var collection = db.GetCollection<ServiceData>(CollectionName);

                    collection.Update(service);
                }
                catch (Exception ex)
                {
                    _errorHandler.AddError(ex);
                }
            }
        }

        public void Remove(Guid id)
        {
            lock (LockLiteDB)
            {
                try
                {
                    using var db = CreateDatabase();
                    var collection = db.GetCollection<ServiceData>(CollectionName);

                    collection.Delete(id);
                }
                catch (Exception ex)
                {
                    _errorHandler.AddError(ex);
                }
            }
        }

        public void Save(ServiceData service)
        {
            lock (LockLiteDB)
            {
                try
                {
                    using var db = CreateDatabase();
                    var collection = db.GetCollection<ServiceData>(CollectionName);

                    collection.Insert(service);
                    collection.EnsureIndex(x => x.Id);
                }
                catch (Exception ex)
                {
                    _errorHandler.AddError(ex);
                }
            }
        }
    }
}
