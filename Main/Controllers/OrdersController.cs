using Application.Commands;
using Application.UseCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly LuckyDrawUseCase usecase;
        private readonly IMediator _mediator;

        public OrdersController(LuckyDrawUseCase luckyDrawUseCase, IMediator mediator)
        {
            usecase = luckyDrawUseCase;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest req)
        {
            try
            {
                var ticket = await _mediator.Send(new CreateTicketCommand(req.OrderId, req.CustomerId, req.Amount));
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
