using Application.Features.Chat.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{sessionId:guid}/history")]
        public async Task<IActionResult> GetHistory(Guid sessionId, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetSessionHistoryQuery(sessionId);
                var result = await _mediator.Send(query, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Geçmiş sorgulanırken hata oluştu.", Details = ex.Message });
            }
        }
    }
}
