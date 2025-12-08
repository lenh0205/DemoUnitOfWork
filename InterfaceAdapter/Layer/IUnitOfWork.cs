using Common.Base;

namespace InterfaceAdapter.Layer
{
    public interface IUnitOfWork : IDisposable, IBaseFactoryImplementation
    {
        Task CommitAsync();
        void Commit();
        void CommitTransaction<TContext>();
        void RollbackTransaction<TContext>();
        void BeginTransaction<TContext>();
    }
}
