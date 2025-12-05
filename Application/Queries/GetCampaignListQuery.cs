using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignListQuery() : IRequest<List<Campaign>>;
}
