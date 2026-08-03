namespace Application.DTOs;

public class ChatResponseDto
{
    public string Reply { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}