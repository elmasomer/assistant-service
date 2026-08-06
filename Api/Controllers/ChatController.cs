using applicaiton.DTOs;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")] 
public class ChatController : ControllerBase
{
   
    [HttpPost]
    public IActionResult SendMessage([FromBody] ChatRequestDTO request)
    {
      
        var response = new ChatResponseDto
        {
            Reply = "Merhaba, ben sahte bir asistanım. Mesajınızı aldım."
        };

        return Ok(response);
    }
}