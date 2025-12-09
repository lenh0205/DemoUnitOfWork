using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerEntryQuery(Guid CampaignId, Guid WinnerEntryId) : IRequest<Entry>;

    public class GetWinnerEntryHandler : IRequestHandler<GetWinnerEntryQuery, Entry>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetWinnerEntryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Entry> Handle(GetWinnerEntryQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetInstance<IEntryRepository>().ListEntriesByCampaign(request.CampaignId).FirstOrDefault(e => e.Id == request.WinnerEntryId);
        }
    }
}
