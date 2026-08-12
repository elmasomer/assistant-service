using System;
using System.Threading.Tasks;
using System.ClientModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.AI;
using Microsoft.Agents.AI;
using OpenAI;
using OpenAI.Chat;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class MafChatService : IMafChatService
    {
        private readonly AIAgent _agent;

        public MafChatService(IConfiguration config)
        {
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
                instructions: "Sen akıllı bir asistansın. Sorulara net ve doğru cevaplar ver."
            );
        }
        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            var response = await _agent.RunAsync(userMessage);
            return response.Text;
        }
    }
}