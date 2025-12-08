using Entities;
using Infrastructure.Base;
using InterfaceAdapter.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(IRepositoryDependencies respositoryDependency) : base(respositoryDependency.ApplicationDbContext)
        {
        }

        private readonly ConcurrentDictionary<Guid, Campaign> _store = new();

        public Campaign? GetById(Guid campaignId) => _store.TryGetValue(campaignId, out var c) ? c : null;
        public void Add(Campaign campaign) => _store.TryAdd(campaign.Id, campaign);
        public IList<Campaign> ListAll() => _store.Values.ToList();
    }
}
