using Common.Interfaces;
using Entities;

namespace Application.UseCase
{
    public class LuckyDrawUseCase
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IEntryRepository _entryRepo;

        public LuckyDrawUseCase(
            ICampaignRepository campainRepo,
            ITicketRepository ticketRepo,
            IEntryRepository entryRepo,
            WinnerSelector winnerSelector
        )
        {
            _campaignRepo = campainRepo;
            _ticketRepo = ticketRepo;
            _entryRepo = entryRepo;
        }

        public Ticket CreateTicketUseCase(Guid orderId, Guid customerId, decimal orderAmount)
        {
            const decimal threshold = 1_000_000m;
            if (orderAmount < threshold)
                throw new InvalidOperationException("Order amount below threshold for ticket issuance.");


            var existing = _ticketRepo.GetByOrderId(orderId);
            if (existing != null) return existing; // idempotent


            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                OrderId = orderId,
                IssuedAt = DateTime.UtcNow
            };
            _ticketRepo.Add(ticket);
            return ticket;
        }
        public Entry SubmitEntryUseCase(Guid campaignId, Guid customerId, Guid ticketId)
        {
            var campaign = _campaignRepo.GetById(campaignId) ?? throw new InvalidOperationException("Campaign not found");
            var now = DateTime.UtcNow;
            if (!campaign.IsActive(now)) throw new InvalidOperationException("Campaign is not active");

            var ticket = _ticketRepo.GetById(ticketId) ?? throw new InvalidOperationException("Ticket not found");
            if (ticket.OwnerId != customerId) throw new InvalidOperationException("Ticket does not belong to customer");
            if (ticket.Consumed) throw new InvalidOperationException("Ticket already consumed");

            var existingCount = _entryRepo.CountEntriesForCampaignByCustomer(campaignId, customerId);
            if (existingCount >= campaign.MaxEntriesPerUser) throw new InvalidOperationException("Max entries per user exceeded");

            // consume ticket
            ticket.Consume();
            _ticketRepo.Update(ticket);

            var entry = new Entry
            {
                Id = Guid.NewGuid(),
                CampaignId = campaignId,
                CustomerId = customerId,
                TicketId = ticketId,
                SubmittedAt = now
            };
            _entryRepo.Add(entry);
            return entry;
        }
        public void GetCampaignAnalyticsQuery()
        {

        }
        public Entry? PickWinnerUseCase(Guid campaignId)
        {
            var entries = _entryRepo.ListEntriesByCampaign(campaignId).ToList();
            if (!entries.Any()) return null;

            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var seed = BitConverter.ToUInt32(bytes, 0);
            var index = (int)(seed % entries.Count);
            return entries[index];
        }
    }
}
