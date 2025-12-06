using Application.Commands;
using Application.Queries;
using Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    [Route("api/campaigns")]
    public class CampaignsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CampaignsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest req)
        {
            try
            {
                var reward = new Reward
                {
                    Type = req.RewardType,
                    Metadata = req.RewardMetadata
                };
                var campaign = await _mediator.Send(new CreateCampaignCommand(req.SellerId, req.Name, req.StartAt.ToUniversalTime(), req.EndAt.ToUniversalTime(), req.MaxEntriesPerUser, reward));
                return Ok(new { campaignId = campaign.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListAll()
        {
            var list = await _mediator.Send(new GetCampaignListQuery());
            return Ok(list);
        }

        [HttpGet("{campaignId}")]
        public async Task<IActionResult> GetById(Guid campaignId)
        {
            try
            {
                var campaign = await _mediator.Send(new GetCampaignByIdQuery(campaignId));

                if (campaign == null) return NotFound();
                return Ok(new
                {
                    id = campaign.Id,
                    name = campaign.Name,
                    startAt = campaign.StartAt,
                    endAt = campaign.EndAt,
                    maxEntriesPerUser = campaign.MaxEntriesPerUser,
                    reward = campaign.Reward?.Metadata
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{campaignId}/entries")]
        public async Task<IActionResult> SubmitEntry(Guid campaignId, [FromBody] SubmitEntryRequest req)
        {
            try
            {
                var entry = await _mediator.Send(new SubmitEntryCommand(campaignId, req.CustomerId, req.TicketId));
                return Ok(new { entryId = entry.Id, submittedAt = entry.SubmittedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{campaignId}/entries")]
        public async Task<IActionResult> ListEntries(Guid campaignId)
        {
            try
            {
                var entries = await _mediator.Send(new GetEntriesListByCampaignIdQuery(campaignId));
                return Ok(entries);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{campaignId}/draw")]
        public async Task<IActionResult> Draw(Guid campaignId)
        {
            try
            {
                var winner = await _mediator.Send(new PickWinnerCommand(campaignId));
                if (winner == null) return NotFound(new { message = "No entries" });

                var winnerEntry = await _mediator.Send(new GetWinnerEntryQuery(campaignId, winner.EntryId));
                return Ok(new { winnerEntryId = winner.EntryId, customerId = winnerEntry?.CustomerId, awardedAt = winner.AwardedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{campaignId}/winner")]
        public async Task<IActionResult> GetWinner(Guid campaignId)
        {
            var winner = await _mediator.Send(new GetWinnerByCampaignIdQuery(campaignId));
            if (winner == null) return NotFound();

            var winnerEntry = await _mediator.Send(new GetWinnerEntryQuery(campaignId, winner.EntryId));
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