using Application.DTOs;

namespace Application.Interfaces
{
    public interface IMafChatService
    {
        Task<ChatResponseDto> GetChatResponseAsync(string userMessage, Guid? sessionId = null, CancellationToken cancellationToken = default);
        Task<SessionHistoryDto> GetSessionHistoryAsync(Guid sessionId, CancellationToken cancellationToken = default);
    }
}