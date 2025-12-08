using Entities;
using InterfaceAdapter.Base;

namespace InterfaceAdapter.Repositories
{
    public interface IWinnerRepository : IBaseRepository<Winner>
    {
        Winner? GetWinnerByCampaign(Guid campaignId);
        void Add(Winner winner);
    }
}
