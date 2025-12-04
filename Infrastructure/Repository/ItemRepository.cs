using Entities;
using Infrastructure.Base;
using Infrastructure.DatabaseContext;
using InterfaceAdapter.Repositories;

namespace Infrastructure.Repository
{
    public class ItemRepository : BaseRepository<Item, IApplicationDbContext>, IRepository<Item>
    {
        public ItemRepository(IRepositoryDependencies respositoryDependency) : base(respositoryDependency.ApplicationDbContext)
        {
        }
    }
}
