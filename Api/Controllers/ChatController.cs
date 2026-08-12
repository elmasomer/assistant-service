using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.Interfaces;
using Application.DTOs;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMafChatService _chatService;

        public ChatController(IMafChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.message))
            {
                return BadRequest(new { Error = "Soru alanı boş olamaz." });
            }

            try
            {
                var rawAnswer = await _chatService.GetChatResponseAsync(request.message);
                var responseDto = new ChatResponseDto(
                    Reply: rawAnswer,
                    Timestamp: DateTime.UtcNow
                );

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Yapay zeka servisine bağlanırken bir hata oluştu.", Details = ex.Message });
            }
        }
    }
}