using Entities;
using InterfaceAdapter.Base;

namespace InterfaceAdapter.Repositories
{
    public interface ICampaignRepository : IBaseRepository<Campaign>
    {
        Campaign? GetById(Guid id);
        void Add(Campaign campaign);
        IList<Campaign> ListAll();
    }
}
