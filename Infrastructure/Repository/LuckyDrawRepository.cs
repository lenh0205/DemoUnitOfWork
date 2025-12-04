using Entities;
using InterfaceAdapter.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly ConcurrentDictionary<Guid, Campaign> _store = new();

        public Campaign? GetById(Guid campaignId) => _store.TryGetValue(campaignId, out var c) ? c : null;
        public void Add(Campaign campaign) => _store.TryAdd(campaign.Id, campaign);
        public IList<Campaign> ListAll() => _store.Values.ToList();
    }

    public class TicketRepository : ITicketRepository
    {
        private readonly ConcurrentDictionary<Guid, Ticket> _store = new();
        public Ticket? GetById(Guid ticketId) => _store.TryGetValue(ticketId, out var t) ? t : null;
        public Ticket? GetByOrderId(Guid orderId) => _store.Values.FirstOrDefault(t => t.OrderId == orderId);
        public void Add(Ticket ticket) => _store.TryAdd(ticket.Id, ticket);
        public void Update(Ticket ticket) => _store[ticket.Id] = ticket;
        public int CountAvailableTickets(Guid customerId) => _store.Values.Count(t => t.OwnerId == customerId && !t.Consumed);
    }

    public class EntryRepository : IEntryRepository
    {
        private readonly ConcurrentDictionary<Guid, Entry> _store = new();
        public void Add(Entry entry) => _store.TryAdd(entry.Id, entry);
        public int CountEntriesForCampaignByCustomer(Guid campaignId, Guid customerId) => _store.Values.Count(e => e.CampaignId == campaignId && e.CustomerId == customerId);
        public IList<Entry> ListEntriesByCampaign(Guid campaignId) => _store.Values.Where(e => e.CampaignId == campaignId).ToList();
    }

    public class WinnerRepository : IWinnerRepository
    {
        private readonly ConcurrentDictionary<Guid, Winner> _store = new();
        public Winner? GetWinnerByCampaign(Guid campaignId) => _store.Values.FirstOrDefault(w => w.CampaignId == campaignId);
        public void Add(Winner winner) => _store.TryAdd(winner.Id, winner);
    }
}
