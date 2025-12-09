using Entities;
using InterfaceAdapter.Layer;
using InterfaceAdapter.Repositories;
using MediatR;
using System.Collections.Concurrent;
using System.Security.Cryptography;

namespace Application.Commands
{
    public record PickWinnerCommand(Guid CampaignId) : IRequest<Winner>;

    public class PickWinnerHandler : IRequestHandler<PickWinnerCommand, Winner>
    {
        private static readonly ConcurrentDictionary<Guid, object> _drawLocks = new();
        private readonly IUnitOfWork _unitOfWork;

        public PickWinnerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Winner> Handle(PickWinnerCommand request, CancellationToken cancellationToken)
        {
            var winnerRepo = _unitOfWork.GetInstance<IWinnerRepository>();
            var entryRepo = _unitOfWork.GetInstance<IEntryRepository>();

            var existingWinner = winnerRepo.GetWinnerByCampaign(request.CampaignId);
            if (existingWinner != null) return existingWinner;

            // obtain in-process lock for this campaign
            var lockObj = _drawLocks.GetOrAdd(request.CampaignId, _ => new object());
            lock (lockObj)
            {
                // Double-check after acquiring lock
                existingWinner = winnerRepo.GetWinnerByCampaign(request.CampaignId);
                if (existingWinner != null) return existingWinner;

                var entries = entryRepo.ListEntriesByCampaign(request.CampaignId).ToList();
                if (!entries.Any()) return null;

                // secure RNG
                using var rng = RandomNumberGenerator.Create();
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var seed = BitConverter.ToUInt32(bytes, 0);
                var index = (int)(seed % entries.Count);
                var chosen = entries[index];

                var winner = new Winner(Guid.NewGuid(), request.CampaignId, chosen.Id, DateTime.UtcNow);
                winnerRepo.InsertAsync(winner);
                _unitOfWork.CommitAsync();
                return winner;
            }
        }
    }
}
