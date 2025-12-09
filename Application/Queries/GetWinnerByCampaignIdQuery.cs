using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerByCampaignIdQuery(Guid CampaignId) : IRequest<Winner>;

    public class GetWinnerByCampaignIdHandler : IRequestHandler<GetWinnerByCampaignIdQuery, Winner>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWinnerByCampaignIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Winner> Handle(GetWinnerByCampaignIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetInstance<IWinnerRepository>().GetWinnerByCampaign(request.CampaignId);
        }
    }
}
