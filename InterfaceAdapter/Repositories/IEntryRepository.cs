using Entities;
using InterfaceAdapter.Base;

namespace InterfaceAdapter.Repositories
{
    public interface IEntryRepository : IBaseRepository<Entry>
    {
        void Add(Entry entry);
        int CountEntriesForCampaignByCustomer(Guid campaignId, Guid customerId);
        IList<Entry> ListEntriesByCampaign(Guid campaignId);
    }
}
