using Application.Base;
using Entities;
using MediatR;

namespace Application.Queries
{
    public record GetWinnerEntryQuery(Guid CampaignId, Guid WinnerEntryId) : IRequest<Entry>;

    public class GetWinnerEntryHandler : BaseBusinessHandler, IRequestHandler<GetWinnerEntryQuery, Entry>
    {
        public GetWinnerEntryHandler(IBusinessHandlerDependencies businessHandlerDependencies) : base(businessHandlerDependencies)
        {
        }

        public async Task<Entry> Handle(GetWinnerEntryQuery request, CancellationToken cancellationToken)
        {
            return _unitOfWork.GetRepository<Entry>().ListEntriesByCampaign(request.CampaignId).FirstOrDefault(e => e.Id == request.WinnerEntryId);
        }
    }
}
