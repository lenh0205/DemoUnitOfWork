using Application.Base;
using Entities;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignListQuery() : IRequest<List<Campaign>>;

    public class GetCampaignListHandler : BaseBusinessHandler, IRequestHandler<GetCampaignListQuery, List<Campaign>>
    {
        public GetCampaignListHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<List<Campaign>> Handle(GetCampaignListQuery request, CancellationToken cancellationToken)
        {
            var list = _unitOfWork.GetInstance<ICampaignRepository>().ListAll().ToList();
            return list;
        }
    }
}
