using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Chat.Queries
{
    public class GetSessionHistoryQueryHandler : IRequestHandler<GetSessionHistoryQuery, SessionHistoryDto>
    {
        private readonly IMafChatService _chatService;

        public GetSessionHistoryQueryHandler(IMafChatService chatService)
        {
            _chatService = chatService;
        }

        public Task<SessionHistoryDto> Handle(GetSessionHistoryQuery request, CancellationToken cancellationToken)
        {
            return _chatService.GetSessionHistoryAsync(request.SessionId, cancellationToken);
        }
    }
}
