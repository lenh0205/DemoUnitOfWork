using Application.Base;
using Entities;
using MediatR;

namespace Application.Commands
{
    public record CreateCampaignCommand(Guid SellerId, string Name, DateTime StartAt, DateTime EndAt, int MaxEntriesPerUser, Reward Reward) : IRequest<Campaign>;

    public class CreateCampaignHandler : BaseBusinessHandler, IRequestHandler<CreateCampaignCommand, Campaign>
    {
        public CreateCampaignHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<Campaign> Handle(CreateCampaignCommand request, CancellationToken cancellationToken)
        {
            if (request.EndAt <= request.StartAt) throw new InvalidOperationException("End time must be after start time.");
            var campaign = new Campaign
            {
                Id = Guid.NewGuid(),
                SellerId = request.SellerId,
                Name = request.Name,
                StartAt = request.StartAt,
                EndAt = request.EndAt,
                MaxEntriesPerUser = request.MaxEntriesPerUser,
                Reward = request.Reward
            };
            await _unitOfWork.GetRepository<Campaign>().InsertAsync(campaign);
            await _unitOfWork.CommitAsync();
            return campaign;
        }
    }
}
