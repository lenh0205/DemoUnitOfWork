using Common.Interfaces;
using Entities;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Application.UseCase
{
    public class LuckyDrawUseCase
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly ITicketRepository _ticketRepo;
        private readonly IEntryRepository _entryRepo;
        private readonly IWinnerRepository _winnerRepo;

        public LuckyDrawUseCase(
            ICampaignRepository campainRepo,
            ITicketRepository ticketRepo,
            IEntryRepository entryRepo,
            IWinnerRepository winnerRepo
        )
        {
            _campaignRepo = campainRepo;
            _ticketRepo = ticketRepo;
            _entryRepo = entryRepo;
            _winnerRepo = winnerRepo;
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
                OwnerId = customerId,
                OrderId = orderId,
                IssuedAt = DateTime.UtcNow
            };
            _ticketRepo.Add(ticket);
            return ticket;
        }

        public Campaign CreateCampaignUseCase(Guid sellerId, string name, DateTime startAt, DateTime endAt, int maxEntriesPerUser, Reward reward)
        {
            if (endAt <= startAt) throw new InvalidOperationException("End time must be after start time.");
            var campaign = new Campaign
            {
                Id = Guid.NewGuid(),
                SellerId = sellerId,
                Name = name,
                StartAt = startAt,
                EndAt = endAt,
                MaxEntriesPerUser = maxEntriesPerUser,
                Reward = reward
            };
            _campaignRepo.Add(campaign);
            return campaign;
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

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<Guid, object> _drawLocks = new();
        public Winner? PickWinnerUseCase(Guid campaignId)
        {
            // if already drawn, return existing
            var existingWinner = _winnerRepo.GetWinnerByCampaign(campaignId);
            if (existingWinner != null) return existingWinner;

            // obtain in-process lock for this campaign
            var lockObj = _drawLocks.GetOrAdd(campaignId, _ => new object());
            lock (lockObj)
            {
                // Double-check after acquiring lock
                existingWinner = _winnerRepo.GetWinnerByCampaign(campaignId);
                if (existingWinner != null) return existingWinner;

                var entries = _entryRepo.ListEntriesByCampaign(campaignId).ToList();
                if (!entries.Any()) return null;

                // secure RNG
                using var rng = RandomNumberGenerator.Create();
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var seed = BitConverter.ToUInt32(bytes, 0);
                var index = (int)(seed % entries.Count);
                var chosen = entries[index];

                var winner = new Winner(Guid.NewGuid(), campaignId, chosen.Id, DateTime.UtcNow);
                _winnerRepo.Add(winner);
                return winner;
            }
        }
    }
}
