using Azure;
using Azure.AI.Inference;
using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI.Chat;
using RAGLib.Models;
using SearchEngineManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib
{
    public class LLMExtensionHelper
    {
        public static string Host { get; set; } = string.Empty;
        public static string ApiKey { get; set; } = string.Empty;
        private string _conversationId = string.Empty;
        //private string _modelName = "gpt-oss:20b";
        //private string _modelName = @"cwchang/llama3-taide-lx-8b-chat-alpha1:latest";
        private string _modelName = @"cwchang/llama-3-taiwan-8b-instruct:latest";

        public async Task ChatingTest(IChatClient chatClient)
        {
            List<Microsoft.Extensions.AI.ChatMessage> chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();

            ChatOptions chatOptions = new();
            chatOptions.ConversationId = Guid.NewGuid().ToString(); // 可以指定對話ID來繼續對話
            string prompt = "請幫我列出咖啡店的廣告文宣。";
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
            string model = _modelName;
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

        // It's for AIChatTest4 function
        private ILBSearchManager _searchManager = new LBSearchManager();

        private void PrepareData()
        {
            var testData = new TestData()
            {
                TextField1 = "這是一個關於C#的入門教材",
                NumField1 = 1,
                LongTextField = "C#是一種現代化、通用型的程式語言，適合用於開發各種應用程式，包括桌面應用、網頁應用和行動應用。"
            };
            _searchManager.CreateIndex(testData);
        }

        class QueryMessage
        {
            public string Query { get; set; } = string.Empty;
        }

        public async Task AIChatTest4()
        {
            PrepareData();

            var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
            string model = _modelName;
            builder.Services.AddChatClient(new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), model));
            var app = builder.Build();
            var chatClient = app.Services.GetRequiredService<IChatClient>();
            string question = "請幫我搜尋C#的入門教材";
            string prompt = $"你現在是一個搜尋引擎專家，請把使用者的問題轉成搜尋引擎使用的關鍵字，並以下列JSON格式表示：\n" +
                "{\"Query\": \"使用者的問題\"}\n" +
                $"使用者的問題如下：{question}";
            var chatCompletion = await chatClient.GetResponseAsync(prompt);
            Console.WriteLine($"Prompt:{prompt}");
            Console.WriteLine();
            List<string> args = new List<string>();

            Console.WriteLine();
            Console.WriteLine("Arg:");
            foreach (var message in chatCompletion.Messages)
            {
                var queryMessage = System.Text.Json.JsonSerializer.Deserialize<QueryMessage>(message.Text);
                args.Add(queryMessage!.Query);
                Console.Write(message.Text);
            }
            Console.WriteLine();

            //LBSearchManager sInstance = new LBSearchManager();
            ILBSearchManager searchManager = _searchManager;
            var ans = searchManager.Search<TestData>(string.Join(',', args));
            string prompt2 = $"根據以下搜尋結果，回答使用者的問題：{question}\n" +
                "搜尋結果內容如下：\n" + string.Join('\n', ans);
            Console.WriteLine();
            Console.WriteLine($"prompt2: {prompt2}");
            Console.WriteLine();
            chatCompletion = await chatClient.GetResponseAsync(prompt2);
            Console.WriteLine($"Prompt:{prompt}");
            Console.WriteLine();
            Console.WriteLine("Final Ans:");
            foreach (var message in chatCompletion.Messages)
            {
                Console.Write(message.Text);
            }
            Console.WriteLine();
        }

        public async Task AIChatTest5()
        {
            LLMHelper lLMHelper = new LLMHelper();
            await lLMHelper.RunAgentsAISampleAsync();
        }

        public async Task AIChatTest6()
        {
            LLMHelper lLMHelper = new LLMHelper();
            await lLMHelper.TestMCPServer();
        }
    }
}
