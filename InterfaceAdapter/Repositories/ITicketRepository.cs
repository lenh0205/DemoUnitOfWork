using Entities;
using InterfaceAdapter.Base;

namespace InterfaceAdapter.Repositories
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Ticket? GetById(Guid id);
        Ticket? GetByOrderId(Guid orderId);
        void Add(Ticket ticket);
        void Update(Ticket ticket);
        int CountAvailableTickets(Guid customerId);
    }
}
