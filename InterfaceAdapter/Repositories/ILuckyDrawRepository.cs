using Entities;
using System.Reflection;

namespace InterfaceAdapter.Repositories
{
    public interface ICampaignRepository
    {
        Campaign? GetById(Guid id);
        void Add(Campaign campaign);
        IList<Campaign> ListAll();
    }
    public interface ITicketRepository
    {
        Ticket? GetById(Guid id);
        Ticket? GetByOrderId(Guid orderId);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        int CountAvailableTickets(Guid customerId);
    }
    public interface IEntryRepository
    {
        void Add(Entry entry);
        int CountEntriesForCampaignByCustomer(Guid campaignId, Guid customerId);
        IList<Entry> ListEntriesByCampaign(Guid campaignId);
    }
    public interface IWinnerRepository
    {
        Winner? GetWinnerByCampaign(Guid campaignId);
        void Add(Winner winner);
    }
}
