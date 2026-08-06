using Application.DTOs;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")] 
public class ChatController : ControllerBase
{
    [HttpPost]
    public IActionResult SendMessage([FromBody] ChatRequestDto request)
    {
        var response = new ChatResponseDto("Merhaba, ben sahte bir asistanım. Mesajınızı aldım.", DateTime.UtcNow);

        return Ok(response);
    }
}