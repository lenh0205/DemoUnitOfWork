using InterfaceAdapter.UnitOfWorks;

namespace Application.Base
{
    public interface IBaseBusinessHandler
    {
        public string TestBaseBusinessHandler();
    }

    public abstract class BaseBusinessHandler : IBaseBusinessHandler
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMongoUnitOfWork _mongoUnitOfWork;

        protected BaseBusinessHandler(IBusinessHandlerDependencies businessHandlerDependencies)
        {
            _unitOfWork = businessHandlerDependencies.UnitOfWork;
            _mongoUnitOfWork = businessHandlerDependencies.MongoUnitOfWork;
        }

        public string TestBaseBusinessHandler()
        {
            return "TestBaseBusinessHandler";
        }
    }
}
