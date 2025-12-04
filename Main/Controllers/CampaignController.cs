using Application.UseCase;
using Entities;
using InterfaceAdapter.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceStack;

namespace Main.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Mvc.Route("api/campaigns")]
    public class CampaignsController : ControllerBase
    {
        private readonly LuckyDrawUseCase usecase;
        private readonly ICampaignRepository _campaignRepo;
        private readonly IEntryRepository _entryRepo;
        private readonly IWinnerRepository _winnerRepo;
        private readonly IMediator _mediator;

        public CampaignsController(
            LuckyDrawUseCase luckyDrawUseCase, 
            ICampaignRepository campaignRepo,
            IEntryRepository entryRepo,
            IWinnerRepository winnerRepo,
            IMediator mediator
        )
        {
            usecase = luckyDrawUseCase;
            _campaignRepo = campaignRepo;
            _entryRepo = entryRepo;
            _winnerRepo = winnerRepo;
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult CreateCampaign([FromBody] CreateCampaignRequest req)
        {
            try
            {
                var reward = new Reward
                {
                    Type = req.RewardType,
                    Metadata = req.RewardMetadata
                };
                var c = usecase.CreateCampaignUseCase(req.SellerId, req.Name, req.StartAt.ToUniversalTime(), req.EndAt.ToUniversalTime(), req.MaxEntriesPerUser, reward);
                return Ok(new { campaignId = c.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ListAll()
        {
            var list = _campaignRepo.ListAll()
                .Select(c => new { id = c.Id, name = c.Name, startAt = c.StartAt, endAt = c.EndAt })
                .ToList();
            return Ok(list);
        }

        [HttpGet("{campaignId}")]
        public IActionResult GetById(Guid campaignId)
        {
            var c = _campaignRepo.GetById(campaignId);
            if (c == null) return NotFound();
            return Ok(new { id = c.Id, name = c.Name, startAt = c.StartAt, endAt = c.EndAt, maxEntriesPerUser = c.MaxEntriesPerUser, reward = c.Reward?.Metadata });
        }

        [HttpPost("{campaignId}/entries")]
        public IActionResult SubmitEntry(Guid campaignId, [FromBody] SubmitEntryRequest req)
        {
            try
            {
                var entry = usecase.SubmitEntryUseCase(campaignId, req.CustomerId, req.TicketId);
                return Ok(new { entryId = entry.Id, submittedAt = entry.SubmittedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{campaignId}/entries")]
        public IActionResult ListEntries(Guid campaignId)
        {
            var entries = _entryRepo.ListEntriesByCampaign(campaignId)
                .Select(e => new { entryId = e.Id, customerId = e.CustomerId, ticketId = e.TicketId, submittedAt = e.SubmittedAt })
                .ToList();
            return Ok(entries);
        }

        [HttpPost("{campaignId}/draw")]
        public IActionResult Draw(Guid campaignId)
        {
            try
            {
                var winner = usecase.PickWinnerUseCase(campaignId);
                if (winner == null) return NotFound(new { message = "No entries" });
                var winnerEntry = _entryRepo.ListEntriesByCampaign(campaignId).FirstOrDefault(e => e.Id == winner.EntryId);
                return Ok(new { winnerEntryId = winner.EntryId, customerId = winnerEntry?.CustomerId, awardedAt = winner.AwardedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{campaignId}/winner")]
        public IActionResult GetWinner(Guid campaignId)
        {
            var winner = _winnerRepo.GetWinnerByCampaign(campaignId);
            if (winner == null) return NotFound();
            var winnerEntry = _entryRepo.ListEntriesByCampaign(campaignId).FirstOrDefault(e => e.Id == winner.EntryId);
            return Ok(new { winnerEntryId = winner.EntryId, customerId = winnerEntry?.CustomerId, awardedAt = winner.AwardedAt });
        }
    }

    public class CreateCampaignRequest
    {
        public Guid SellerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public int MaxEntriesPerUser { get; set; } = 1;
        public RewardType RewardType { get; set; } = RewardType.Coupon;
        public string RewardMetadata { get; set; } = string.Empty;
    }

    public class SubmitEntryRequest
    {
        public Guid CustomerId { get; set; }
        public Guid TicketId { get; set; }
    }
}