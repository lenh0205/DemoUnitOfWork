namespace Common.Interfaces
{
    public interface IMongoUnitOfWork : IDisposable
    {
        IMongoRepository<T> GetRepository<T>();
        Task<bool> Commit();
    }
}
