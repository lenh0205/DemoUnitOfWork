using Entities;
using Infrastructure.Base;
using InterfaceAdapter.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public class EntryRepository : BaseRepository<Entry>,IEntryRepository
    {
        public EntryRepository(IRepositoryDependencies respositoryDependency) : base(respositoryDependency.ApplicationDbContext)
        {
        }

        private readonly ConcurrentDictionary<Guid, Entry> _store = new();
        public void Add(Entry entry) => _store.TryAdd(entry.Id, entry);
        public int CountEntriesForCampaignByCustomer(Guid campaignId, Guid customerId) => _store.Values.Count(e => e.CampaignId == campaignId && e.CustomerId == customerId);
        public IList<Entry> ListEntriesByCampaign(Guid campaignId) => _store.Values.Where(e => e.CampaignId == campaignId).ToList();
    }
}
