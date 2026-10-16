namespace Application.DTOs;

public record ChatResponseDto(Guid SessionId, string SessionTitle, string Reply, DateTime Timestamp);