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
        public Guid Type { get; set; }
        public string? Metadata { get; set; } = string.Empty;
    }

    public class WinnerSelector
    {
        public Entry PickRandomWinner(List<Entry> entries)
        {
            return entries[0];
        }
    }
}
