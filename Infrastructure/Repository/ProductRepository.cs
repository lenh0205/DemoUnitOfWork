using Entities;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;

namespace Infrastructure.Repository
{
    public class ProductRepository : BaseMongoRepository<Product>, IMongoRepository<Product>
    {
        public ProductRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
