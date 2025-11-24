using Application.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    [Route("api/campaigns")]
    public class CampaignsController : ControllerBase
    {
        private readonly LuckyDrawUseCase usecase;

        public CampaignsController(LuckyDrawUseCase luckyDrawUseCase)
        {
            usecase = luckyDrawUseCase;
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

        [HttpPost("{campaignId}/draw")]
        public IActionResult Draw(Guid campaignId)
        {
            var winner = usecase.PickWinnerUseCase(campaignId);
            if (winner == null) return NotFound(new { message = "No entries" });
            return Ok(new { winnerEntryId = winner.Id, customerId = winner.CustomerId });
        }
    }

    public class SubmitEntryRequest
    {
        public Guid CustomerId { get; set; }
        public Guid TicketId { get; set; }
    }
}