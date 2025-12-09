using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Commands
{
    public record CreateCampaignCommand(Guid SellerId, string Name, DateTime StartAt, DateTime EndAt, int MaxEntriesPerUser, Reward Reward) : IRequest<Campaign>;

    public class CreateCampaignHandler : IRequestHandler<CreateCampaignCommand, Campaign>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCampaignHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.GetInstance<ICampaignRepository>().InsertAsync(campaign);
            await _unitOfWork.CommitAsync();
            return campaign;
        }
    }
}
