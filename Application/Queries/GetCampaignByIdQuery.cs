using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignByIdQuery(Guid CampaignId) : IRequest<Campaign>;

    public class GetCampaignByIdHandler : IRequestHandler<GetCampaignByIdQuery, Campaign>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampaignByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Campaign> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetInstance<ICampaignRepository>().GetById(request.CampaignId);
        }
    }
}
