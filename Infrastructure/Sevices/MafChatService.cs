using Application.Interfaces;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace Infrastructure.Services
{
    public class MafChatService : IMafChatService
    {
        private readonly IChatCompletionService _chatCompletionService;

        public MafChatService(Kernel kernel)
        {
            _chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            try
            {
                var chatHistory = new ChatHistory();
                chatHistory.AddUserMessage(userMessage);

                var result = await _chatCompletionService.GetChatMessageContentAsync(chatHistory);

                return result.Content ?? "Boş yanıt döndü.";
            }
            catch (Exception ex)
            {
                return $"[MAF SİSTEM HATASI]: {ex.Message}";
            }
        }
    }
}