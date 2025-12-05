using Entities;
using MediatR;

namespace Application.Commands
{
    public record PickWinnerCommand(Guid CampaignId) : IRequest<Winner>;
}
