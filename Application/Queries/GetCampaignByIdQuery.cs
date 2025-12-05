using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignByIdQuery(Guid CampaignId) : IRequest<Campaign>;
}
