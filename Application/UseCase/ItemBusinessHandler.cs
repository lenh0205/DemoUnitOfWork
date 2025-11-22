using Application.Base;
using Entities;

namespace Application.UseCase
{
    public interface IItemBusinessHandler : IBaseBusinessHandler
    {
        public Item GetOne();
    }

    public class ItemBusinessHandler : BaseBusinessHandler, IItemBusinessHandler
    {
        public ItemBusinessHandler(IBusinessHandlerDependencies serviceProvider) : base(serviceProvider)
        {
        }

        public Item GetOne()
        {
            var result = _unitOfWork.GetRepository<Item>().GetByID(Guid.NewGuid());
            return result;
        }
    }
}
