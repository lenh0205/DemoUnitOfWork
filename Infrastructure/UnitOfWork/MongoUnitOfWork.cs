using Common.Base;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;
using InterfaceAdapter.UnitOfWorks;

namespace Infrastructure.UnitOfWork
{
    public class MongoUnitOfWork : BaseFactoryImplementation<IRepositoryDependencies>, IMongoUnitOfWork
    {
        private readonly IMongoDbContext _context;

        public MongoUnitOfWork(IRepositoryDependencies repositoryDependencies) : base(repositoryDependencies)
        {
            _context = repositoryDependencies.MongoDbContext;
        }

        public IMongoRepository<T> GetRepository<T>() => GetInstance<IMongoRepository<T>>();

        public async Task<bool> Commit()
        {
            var changeAmount = await _context.SaveChanges();

            return changeAmount > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
