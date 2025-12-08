using Application.Base;
using Entities;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignByIdQuery(Guid CampaignId) : IRequest<Campaign>;

    public class GetCampaignByIdHandler : BaseBusinessHandler, IRequestHandler<GetCampaignByIdQuery, Campaign>
    {
        public GetCampaignByIdHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<Campaign> Handle(GetCampaignByIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetInstance<ICampaignRepository>().GetById(request.CampaignId);
        }
    }
}
