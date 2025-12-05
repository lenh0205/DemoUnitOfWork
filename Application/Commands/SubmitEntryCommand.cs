using Entities;
using MediatR;

namespace Application.Commands
{
    public record SubmitEntryCommand(Guid CampaignId, Guid CustomerId, Guid TicketId) : IRequest<Entry>;
}
