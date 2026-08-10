using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMafChatService _chatService;

        public ChatController(IMafChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequestDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.message))
            {
                return BadRequest(new { error = "Message alanı boş olamaz." });
            }

            var response = await _chatService.GetChatResponseAsync(request.message);
            return Ok(new { reply = response, timestamp = DateTime.UtcNow });
        }
    }
}