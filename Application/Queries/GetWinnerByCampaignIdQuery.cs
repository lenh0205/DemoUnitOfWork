using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerByCampaignIdQuery(Guid CampaignId) : IRequest<Winner>;
}
