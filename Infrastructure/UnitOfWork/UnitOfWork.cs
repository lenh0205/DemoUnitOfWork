using Common.Base;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;
using InterfaceAdapter.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : BaseFactoryImplementation<IRepositoryDependencies>, IUnitOfWork
    {
        private readonly IApplicationDbContext _applicationDbcontext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(IRepositoryDependencies repositoryDependencies) : base(repositoryDependencies)
        {
            _applicationDbcontext = repositoryDependencies.ApplicationDbContext;
        }

        #region ----------> Config Repository
        public IRepository<T> GetRepository<T>() => GetInstance<IRepository<T>>();

        #endregion


        #region ----------> UnitOfWork Processing

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _applicationDbcontext.Dispose();
                    _transaction?.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Commit()
        {
            _applicationDbcontext.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _applicationDbcontext.SaveChangesAsync();
        }
        public void BeginTransaction<TContext>()
        {
            if (typeof(TContext) == typeof(ApplicationDbContext))
                _transaction = _applicationDbcontext.Database.BeginTransaction();
        }
        public void CommitTransaction<TContext>() => _transaction?.Commit();
        public void RollbackTransaction<TContext>() => _transaction?.Rollback();

        #endregion
    }
}
