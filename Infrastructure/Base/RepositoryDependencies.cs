
using Infrastructure.DatabaseContext;

namespace Infrastructure.Base
{
    public interface IRepositoryDependencies
    {
        public IApplicationDbContext ApplicationDbContext { get; }
        public IMongoDbContext MongoDbContext { get; }
    }

    public class RepositoryDependencies : IRepositoryDependencies
    {
        public IApplicationDbContext ApplicationDbContext { get; }
        public IMongoDbContext MongoDbContext { get; }

        public RepositoryDependencies(IApplicationDbContext applicationDbContext, IMongoDbContext mongoDbContext)
        {
            ApplicationDbContext = applicationDbContext;
            MongoDbContext = mongoDbContext;
        }
    }
}
