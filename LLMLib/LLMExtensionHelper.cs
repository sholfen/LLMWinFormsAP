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

        public async Task AIChatTest()
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            string model = "gemma";
            builder.Services.AddChatClient(new OllamaChatClient(new Uri("http://localhost:11434"), model));
            var app = builder.Build();
            var chatClient = app.Services.GetRequiredService<IChatClient>();
            var chatCompletion = await chatClient.GetResponseAsync("什麼是女僕咖啡店");
            foreach (var message in chatCompletion.Messages)
            {
                Console.Write(message.Text);
            }
            Console.WriteLine();
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
    }
}
