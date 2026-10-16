using Application.DTOs;
using MediatR;

namespace Application.Features.Chat.Commands
{
    public class AskChatCommand : IRequest<ChatResponseDto>
    {
        public string Message { get; }

        public Guid? SessionId { get; }

        public AskChatCommand(string message, Guid? sessionId = null)
        {
            Message = message;
            SessionId = sessionId;
        }
    }
}