using InterfaceAdapter.Repositories;

namespace InterfaceAdapter.UnitOfWorks
{
    public interface IMongoUnitOfWork : IDisposable
    {
        IMongoRepository<T> GetRepository<T>();
        Task<bool> Commit();
    }
}
