using Entities;
using Infrastructure.Base;
using InterfaceAdapter.Repositories;
using System.Collections.Concurrent;

namespace Infrastructure.Repository
{
    public class TicketRepository :  BaseRepository<Ticket>,ITicketRepository
    {
        public TicketRepository(IRepositoryDependencies respositoryDependency) : base(respositoryDependency.ApplicationDbContext)
        {
        }

        private readonly ConcurrentDictionary<Guid, Ticket> _store = new();
        public Ticket? GetById(Guid ticketId) => _store.TryGetValue(ticketId, out var t) ? t : null;
        public Ticket? GetByOrderId(Guid orderId) => _store.Values.FirstOrDefault(t => t.OrderId == orderId);
        public void Add(Ticket ticket) => _store.TryAdd(ticket.Id, ticket);
        public void Update(Ticket ticket) => _store[ticket.Id] = ticket;
        public int CountAvailableTickets(Guid customerId) => _store.Values.Count(t => t.OwnerId == customerId && !t.Consumed);
    }
}
