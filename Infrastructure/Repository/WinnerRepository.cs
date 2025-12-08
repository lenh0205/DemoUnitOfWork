using Entities;
using Infrastructure.Base;
using InterfaceAdapter.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public class WinnerRepository : BaseRepository<Winner>, IWinnerRepository
    {
        public WinnerRepository(IRepositoryDependencies respositoryDependency) : base(respositoryDependency.ApplicationDbContext)
        {
        }

        private readonly ConcurrentDictionary<Guid, Winner> _store = new();
        public Winner? GetWinnerByCampaign(Guid campaignId) => _store.Values.FirstOrDefault(w => w.CampaignId == campaignId);
        public void Add(Winner winner) => _store.TryAdd(winner.Id, winner);
    }
}
