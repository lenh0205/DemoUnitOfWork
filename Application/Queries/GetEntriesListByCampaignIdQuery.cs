using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetEntriesListByCampaignIdQuery(Guid CampaignId) : IRequest<List<Entry>>;

    public class GetEntriesListByCampaignIdHandler : IRequestHandler<GetEntriesListByCampaignIdQuery, List<Entry>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEntriesListByCampaignIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Entry>> Handle(GetEntriesListByCampaignIdQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetInstance<IEntryRepository>().ListEntriesByCampaign(request.CampaignId).ToList();
        }
    }
}
