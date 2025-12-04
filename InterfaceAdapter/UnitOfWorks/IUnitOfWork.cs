using Common.Base;
using InterfaceAdapter.Repositories;

namespace InterfaceAdapter.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable, IBaseFactoryImplementation
    {
        IRepository<T> GetRepository<T>();

        // for IDisposable implementation
        Task CommitAsync();
        void Commit();
        void CommitTransaction<TContext>();
        void RollbackTransaction<TContext>();
        void BeginTransaction<TContext>();
    }
}
