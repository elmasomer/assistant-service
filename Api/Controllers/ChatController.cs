using Application.DTOs;
using Application.Features.Chat.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new AskChatCommand(request.Message, request.SessionId);
                var responseDto = await _mediator.Send(command, cancellationToken);

                return Ok(responseDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Yapay zeka servisine bağlanırken bir hata oluştu.", Details = ex.Message });
            }
        }
    }
}