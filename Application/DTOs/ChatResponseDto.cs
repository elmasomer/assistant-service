namespace applicaiton.DTOs;
public record ChatResponseDto
{
    public string Reply { get; init; }
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}