using Application.Base;
using Entities;
using MediatR;

namespace Application.Commands
{
    public record CreateTicketCommand(Guid OrderId, Guid CustomerId, decimal OrderAmount) : IRequest<Ticket>;

    public class CreateTicketHandler : BaseBusinessHandler, IRequestHandler<CreateTicketCommand, Ticket>
    {
        public CreateTicketHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<Ticket> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            const decimal threshold = 1_000_000m;
            if (request.OrderAmount < threshold)
                throw new InvalidOperationException("Order amount below threshold for ticket issuance.");

            var ticketRepo = _unitOfWork.GetRepository<Ticket>();
            var existing = ticketRepo.GetByOrderId(request.OrderId);
            ticketRepo.GetByID(request.OrderId);
            if (existing != null) return existing; // idempotent

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                OwnerId = request.CustomerId,
                OrderId = request.OrderId,
                IssuedAt = DateTime.UtcNow
            };
            await ticketRepo.InsertAsync(ticket);
            await _unitOfWork.CommitAsync();
            return ticket;
        }
    }
}
