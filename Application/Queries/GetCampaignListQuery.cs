using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetCampaignListQuery() : IRequest<List<Campaign>>;

    public class GetCampaignListHandler : IRequestHandler<GetCampaignListQuery, List<Campaign>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCampaignListHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Campaign>> Handle(GetCampaignListQuery request, CancellationToken cancellationToken)
        {
            var list = _unitOfWork.GetInstance<ICampaignRepository>().ListAll().ToList();
            return list;
        }
    }
}
