using Application.Base;
using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerByCampaignIdQuery(Guid CampaignId) : IRequest<Winner>;

    public class GetWinnerByCampaignIdHandler : BaseBusinessHandler, IRequestHandler<GetWinnerByCampaignIdQuery, Winner>
    {
        public GetWinnerByCampaignIdHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<Winner> Handle(GetWinnerByCampaignIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Winner>().GetWinnerByCampaign(request.CampaignId);
        }
    }
}
