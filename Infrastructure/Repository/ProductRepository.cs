using Common.Interfaces;
using Entities;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;

namespace Infrastructure.Repository
{
    public class ProductRepository : BaseMongoRepository<Product>, IMongoRepository<Product>
    {
        public ProductRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
