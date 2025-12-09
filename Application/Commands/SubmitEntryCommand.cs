using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Commands
{
    public record SubmitEntryCommand(Guid CampaignId, Guid CustomerId, Guid TicketId) : IRequest<Entry>;

    public class SubmitEntryHandler : IRequestHandler<SubmitEntryCommand, Entry>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitEntryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Entry> Handle(SubmitEntryCommand request, CancellationToken cancellationToken)
        {
            var campaignRepo = _unitOfWork.GetInstance<ICampaignRepository>();
            var ticketRepo = _unitOfWork.GetInstance<ITicketRepository>();
            var entryRepo = _unitOfWork.GetInstance<IEntryRepository>();

            var campaign = campaignRepo.GetById(request.CampaignId) ?? throw new InvalidOperationException("Campaign not found");
            var now = DateTime.UtcNow;
            if (!campaign.IsActive(now)) throw new InvalidOperationException("Campaign is not active");

            var ticket = ticketRepo.GetById(request.TicketId) ?? throw new InvalidOperationException("Ticket not found");
            if (ticket.OwnerId != request.CustomerId) throw new InvalidOperationException("Ticket does not belong to customer");
            if (ticket.Consumed) throw new InvalidOperationException("Ticket already consumed");

            var existingCount = entryRepo.CountEntriesForCampaignByCustomer(request.CampaignId, request.CustomerId);
            if (existingCount >= campaign.MaxEntriesPerUser) throw new InvalidOperationException("Max entries per user exceeded");

            ticket.Consume();
            ticketRepo.Update(ticket);

            var entry = new Entry
            {
                Id = Guid.NewGuid(),
                CampaignId = request.CampaignId,
                CustomerId = request.CustomerId,
                TicketId = request.TicketId,
                SubmittedAt = now
            };
            await entryRepo.InsertAsync(entry);
            await _unitOfWork.CommitAsync();
            return entry;
        }
    }
}
