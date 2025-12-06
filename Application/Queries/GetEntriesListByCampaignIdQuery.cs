using Application.Base;
using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetEntriesListByCampaignIdQuery(Guid CampaignId) : IRequest<List<Entry>>;

    public class GetEntriesListByCampaignIdHandler : BaseBusinessHandler, IRequestHandler<GetEntriesListByCampaignIdQuery, List<Entry>>
    {
        public GetEntriesListByCampaignIdHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<List<Entry>> Handle(GetEntriesListByCampaignIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Entry>().ListEntriesByCampaign(request.CampaignId).ToList();
        }
    }
}
