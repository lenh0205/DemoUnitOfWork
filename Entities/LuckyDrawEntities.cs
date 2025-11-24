namespace Entities
{
    public class Campaign
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public string? Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int MaxEntriesPerUser { get; set; }
        public Reward? Reward { get; set; }
        public List<Entry>? Entries { get; set; }
        public bool IsActive(DateTime now)
        {
            return false;
        }
        public void ValidateEntry(Guid customerId)
        {
        }
    }

    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; } // CustomerId
        public Guid? OrderId { get; set; }
        public bool Consumed { get; set; }
        public DateTime IssuedAt { get; set; }
        public void Consume()
        {
            if (Consumed) throw new InvalidOperationException("Ticket already consumed");
            Consumed = true;
        }
    }

    public class Entry
    {
        public Guid Id { get; set; }
        public Guid CampaignId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid TicketId { get; set; }
        public DateTime SubmittedAt { get; set; }
    }

    public class Reward
    {
        public Guid Id { get; set; }
        public RewardType Type { get; set; }
        public string? Metadata { get; set; } = string.Empty;
    }

    public class Winner
    {
        public Guid Id { get; private set; }
        public Guid CampaignId { get; private set; }
        public Guid EntryId { get; private set; }
        public DateTime AwardedAt { get; private set; }

        public Winner(Guid winnerId, Guid campaignId, Guid entryId, DateTime awardedAt)
        {
            Id = winnerId;
            CampaignId = campaignId;
            EntryId = entryId;
            AwardedAt = awardedAt;
        }
    }

    public enum RewardType
    {
        Coupon
    }
}
