using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib
{
    public class LLMExtensionHelper
    {
        public static string Host { get; set; } = string.Empty;
        public static string ApiKey { get; set; } = string.Empty;
        private string _conversationId = string.Empty;

        public async Task ChatingTest(IChatClient chatClient)
        {
            List<Microsoft.Extensions.AI.ChatMessage> chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();

            ChatOptions chatOptions = new();
            chatOptions.ConversationId = Guid.NewGuid().ToString(); // 可以指定對話ID來繼續對話
            string prompt= "請幫我列出咖啡店的廣告文宣。";
            //_chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.User, prompt));
            Microsoft.Extensions.AI.ChatMessage chatMessage = new(Microsoft.Extensions.AI.ChatRole.User, prompt);
            chatHistory.Add(chatMessage);
            var chatCompletion = await chatClient.GetResponseAsync(chatHistory, chatOptions);
            Console.WriteLine($"Prompt:{prompt}");
            Console.WriteLine();
            foreach (var message in chatCompletion.Messages)
            {
                Console.Write(message.Text);
            }
            Console.WriteLine();
            //chatOptions.ConversationId = chatCompletion.ConversationId; // 紀錄對話ID以便後續使用
            Console.WriteLine($"Conversation ID: {chatOptions.ConversationId}");
            Console.WriteLine($"Response ID: {chatCompletion.ResponseId}");
            Console.WriteLine();
            chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant, chatCompletion.Text));

            prompt = "幫我把前述的文宣寫得精簡一點";
            chatMessage = new(Microsoft.Extensions.AI.ChatRole.User, prompt);
            chatHistory.Add(chatMessage);
            Console.WriteLine($"Prompt:{prompt}");
            Console.WriteLine("2nd response:");
            chatCompletion = await chatClient.GetResponseAsync(chatHistory, chatOptions);
            foreach (var message in chatCompletion.Messages)
            {
                Console.Write(message.Text);
            }
            Console.WriteLine();
            Console.WriteLine($"Conversation ID: {chatOptions.ConversationId}");
            Console.WriteLine($"Response ID: {chatCompletion.ResponseId}");
            Console.WriteLine();
        }

        public async Task AIChatTest()
        {
            List<Microsoft.Extensions.AI.ChatMessage> chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            string model = "gemma";
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:8888"; // Redis server address
                options.InstanceName = "LLMChatCache"; // Instance name for cache
            });
            //builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), model))
            //    .UseDistributedCache();
            builder.Services.AddChatClient(new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), model))
                .UseDistributedCache();
            var app = builder.Build();
            var chatClient = app.Services.GetRequiredService<IChatClient>();

            await ChatingTest(chatClient);
        }

        public async Task AIChatTest2()
        {
            string key = ApiKey;
            string deploymentName = "gpt-4o-mini";
            var endpoint = new Uri(Host);
            var credential = new AzureKeyCredential(key);
            var client = new ChatCompletionsClient(
                endpoint,
                credential,
                new AzureAIInferenceClientOptions()
            ).AsIChatClient(deploymentName);
            Console.WriteLine(await client.GetResponseAsync("什麼是女僕咖啡店"));
        }

        public async Task AIChatTest3()
        {
            string key = ApiKey;
            string deploymentName = "gpt-4o-mini";
            var endpoint = new Uri(Host);
            var credential = new AzureKeyCredential(key);

            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:8888"; // Redis server address
                options.InstanceName = "LLMChatCache"; // Instance name for cache
            });
            builder.Services.AddChatClient(new ChatCompletionsClient(
                endpoint,
                credential,
                new AzureAIInferenceClientOptions()).AsIChatClient(deploymentName))
                .UseDistributedCache();
            var app = builder.Build();
            var chatClient = app.Services.GetRequiredService<IChatClient>();

            await ChatingTest(chatClient);
        }
    }
}
