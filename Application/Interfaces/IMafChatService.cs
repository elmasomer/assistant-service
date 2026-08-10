
namespace Application.Interfaces
{
    public interface IMafChatService
    {
        Task<string> GetChatResponseAsync(string userMessage);
    }
}