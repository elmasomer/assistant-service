namespace Application.DTOs;

public record SessionHistoryDto(
    Guid SessionId,
    string Title,
    DateTime CreatedAt,
    IEnumerable<ChatMessageDto> Messages
);
