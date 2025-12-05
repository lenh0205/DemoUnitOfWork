using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerEntryQuery(Guid CampaignId, Guid WinnerEntryId) : IRequest<Entry>;
}
