using Application.DTOs;
using MediatR;

namespace Application.Features.Chat.Queries
{
    public record GetSessionHistoryQuery(Guid SessionId) : IRequest<SessionHistoryDto>;
}
