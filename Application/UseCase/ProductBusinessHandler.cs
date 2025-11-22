using Application.Base;
using Entities;

namespace Application.UseCase
{
    public interface IProductBusinessHandler : IBaseBusinessHandler
    {
        Task<Product> GetOne();
    }

    public class ProductBusinessHandler : BaseBusinessHandler, IProductBusinessHandler
    {
        public ProductBusinessHandler(IBusinessHandlerDependencies serviceProvider) : base(serviceProvider)
        {
        }

        public async Task<Product> GetOne()
        {
            var result = await _mongoUnitOfWork.GetRepository<Product>().GetById(Guid.NewGuid());
            return result;
        }
    }
}
