using Entities;
using MediatR;

namespace Application.Commands
{
    public record CreateCampaignCommand(Guid SellerId, string Name, DateTime StartAt, DateTime EndAt, int MaxEntriesPerUser, Reward Reward) : IRequest<Campaign>;
}
