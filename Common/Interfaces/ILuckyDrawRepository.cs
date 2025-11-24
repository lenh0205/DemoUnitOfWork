using Entities;

namespace Common.Interfaces
{
    public interface ICampaignRepository
    {
        Campaign? GetById(Guid CampaignId);
        void Add(Campaign campaign);
    }

    public interface IEntryRepository
    {
        void Add(Entry entry);
        int CountEntriesForCampaignByCustomer(Guid campaignId, Guid customerId);
        IList<Entry> ListEntriesByCampaign(Guid campaignId);
    }

    public interface ITicketRepository
    {
        Ticket? GetById(Guid TicketId);
        Ticket? GetByOrderId(Guid orderId);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        int CountAvailableTickets(Guid CustomerId);
    }

    public interface INotificationService
    {
    }
}
