using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Chat.Commands
{
    public class AskChatCommandHandler : IRequestHandler<AskChatCommand, ChatResponseDto>
    {
        private readonly IMafChatService _chatService;

        public AskChatCommandHandler(IMafChatService chatService)
        {
            _chatService = chatService;
        }

        public Task<ChatResponseDto> Handle(AskChatCommand request, CancellationToken cancellationToken)
        {
            return _chatService.GetChatResponseAsync(request.Message, request.SessionId, cancellationToken);
        }
    }
}