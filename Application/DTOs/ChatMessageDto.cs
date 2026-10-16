namespace Application.DTOs;

public record ChatMessageDto(Guid Id, string Role, string Content, DateTime CreatedAt);
