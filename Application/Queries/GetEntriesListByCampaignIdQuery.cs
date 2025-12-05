using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetEntriesListByCampaignIdQuery(Guid CampaignId) : IRequest<List<Entry>>;
}
