using Application.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly LuckyDrawUseCase usecase;

        public OrdersController(LuckyDrawUseCase luckyDrawUseCase)
        {
            usecase = luckyDrawUseCase;
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest req)
        {
            try
            {
                var ticket = usecase.CreateTicketUseCase(req.OrderId, req.CustomerId, req.Amount);
                return Ok(new { ticketId = ticket.Id, issuedAt = ticket.IssuedAt });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class CreateOrderRequest
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
    }
}
