
using Common.Interfaces;

namespace Application.Base
{
    public interface IBusinessHandlerDependencies
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMongoUnitOfWork MongoUnitOfWork { get; }
    }

    public class BusinessHandlerDependencies : IBusinessHandlerDependencies
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMongoUnitOfWork MongoUnitOfWork { get; }

        public BusinessHandlerDependencies(IUnitOfWork unitOfWork, IMongoUnitOfWork mongoUnitOfWork)
        {
            UnitOfWork = unitOfWork;
            MongoUnitOfWork = mongoUnitOfWork;
        }
    }
}
