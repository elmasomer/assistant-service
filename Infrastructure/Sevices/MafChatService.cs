using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using System.ClientModel;

namespace Infrastructure.Services
{
    public class MafChatService : IMafChatService
    {
        private const string UserRole = "user";
        private const string AssistantRole = "assistant";
        private readonly AIAgent _agent;
        private readonly ApplicationDbContext _dbContext;

        public MafChatService(IConfiguration config, ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            var apiKey = config["OpenRouterApiKey"] ?? throw new Exception("API Key bulunamadı!");

            var options = new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1/")
            };

            OpenAIClient client = new OpenAIClient(new ApiKeyCredential(apiKey), options);

            var nativeChatClient = client.GetChatClient("openai/gpt-4o-mini");

            IChatClient chatClient = nativeChatClient.AsIChatClient();

            _agent = new ChatClientAgent(
                chatClient,
                name: "ChatAssistant",
                instructions: "Sen akıllı bir asistansın. Sorulara net ve doğru cevaplar ver. Cevaplarını Türkçe ver ve sohbet geçmişini dikkate al."
            );
        }

        public async Task<ChatResponseDto> GetChatResponseAsync(string userMessage, Guid? sessionId = null, CancellationToken cancellationToken = default)
        {
            var session = await GetOrCreateSessionAsync(userMessage, sessionId, cancellationToken);

            var historyMessages = await _dbContext.ChatMessages
                .AsNoTracking()
                .Where(message => message.ChatSessionId == session.Id)
                .OrderBy(message => message.CreatedAt)
                .Select(message => $"{GetRoleLabel(message.Role)}: {message.Content}")
                .ToListAsync(cancellationToken);

            historyMessages.Add($"Kullanıcı: {userMessage}");

            var prompt = $"""
Oturum bazlı bir asistan olarak yanıt ver.
Sohbet başlığı: {session.Title}

Sohbet geçmişi:
{string.Join(Environment.NewLine, historyMessages)}

Yeni kullanıcı mesajı:
{userMessage}
""";

            var response = await _agent.RunAsync(prompt);
            var reply = response.Text ?? string.Empty;

            _dbContext.ChatMessages.Add(new Domain.Entities.ChatMessage
            {
                ChatSessionId = session.Id,
                Role = UserRole,
                Content = userMessage
            });

            _dbContext.ChatMessages.Add(new Domain.Entities.ChatMessage
            {
                ChatSessionId = session.Id,
                Role = AssistantRole,
                Content = reply
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ChatResponseDto(
                SessionId: session.Id,
                SessionTitle: session.Title,
                Reply: reply,
                Timestamp: DateTime.UtcNow
            );
        }

        private async Task<ChatSession> GetOrCreateSessionAsync(string userMessage, Guid? sessionId, CancellationToken cancellationToken)
        {
            if (sessionId.HasValue)
            {
                var existingSession = await _dbContext.ChatSessions
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.Id == sessionId.Value, cancellationToken);

                if (existingSession is null)
                {
                    throw new KeyNotFoundException("Sohbet oturumu bulunamadı.");
                }

                return existingSession;
            }

            var newSession = new ChatSession
            {
                Title = BuildSessionTitle(userMessage)
            };

            _dbContext.ChatSessions.Add(newSession);
            return newSession;
        }

        private static string BuildPrompt(ChatSession session, string userMessage)
        {
            var history = session.Messages
                .OrderBy(message => message.CreatedAt)
                .Select(message => $"{GetRoleLabel(message.Role)}: {message.Content}")
                .ToList();

            var transcript = history.Count == 0
                ? "Geçmiş mesaj yok."
                : string.Join(Environment.NewLine, history);

            return $"""
Oturum bazlı bir asistan olarak yanıt ver.
Sohbet başlığı: {session.Title}

Sohbet geçmişi:
{transcript}

Yeni kullanıcı mesajı:
{userMessage}
""";
        }

        public async Task<Application.DTOs.SessionHistoryDto> GetSessionHistoryAsync(Guid sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _dbContext.ChatSessions
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

            if (session is null)
                throw new KeyNotFoundException("Sohbet oturumu bulunamadı.");

            var messages = session.Messages
                .OrderBy(m => m.CreatedAt)
                .Select(m => new Application.DTOs.ChatMessageDto(m.Id, m.Role, m.Content, m.CreatedAt))
                .ToList();

            return new Application.DTOs.SessionHistoryDto(session.Id, session.Title, session.CreatedAt, messages);
        }

        private static string BuildSessionTitle(string userMessage)
        {
            var normalized = string.Join(' ', userMessage.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Trim();

            if (string.IsNullOrWhiteSpace(normalized))
            {
                return "Yeni Sohbet";
            }

            const int maxLength = 60;
            return normalized.Length <= maxLength
                ? normalized
                : $"{normalized[..maxLength].Trim()}...";
        }

        private static string GetRoleLabel(string role)
        {
            return role.Equals(UserRole, StringComparison.OrdinalIgnoreCase) ? "Kullanıcı" : "Asistan";
        }
    }
}