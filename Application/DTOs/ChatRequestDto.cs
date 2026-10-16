namespace Application.DTOs;

public record ChatRequestDto(string Message, Guid? SessionId = null);