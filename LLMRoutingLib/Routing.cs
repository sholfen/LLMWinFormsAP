using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp.Models;
using System.ClientModel;

namespace LLMRoutingLib
{
    public enum ModelType
    {
        Llama3,
        Mistral,
        OpenAI,
        AzureOpenAI,
        Claude,
        Other
    }

    public class Routing
    {
        private string _modelName = string.Empty;
        private IChatClient _chatClient;
        private List<ModelTarget> _modelTypes;

        private static readonly IConfiguration _config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        public Routing() : this(@"llama3.1:latest")
        {

        }

        public Routing(string modelName)
        {

            if (string.IsNullOrEmpty(modelName))
            {
                _modelName = @"llama3.1:latest";
            }
            else
            {
                _modelName = modelName;
            }

            _modelTypes = _config.GetSection("ModelTarget").GetChildren().Select(c => new ModelTarget
            {
                Type = c["Type"] ?? string.Empty,
                ModelName = c["ModelName"] ?? string.Empty,
                Provider = c["Provider"] ?? "Ollama"
            }).ToList();

            _chatClient = CreateOllamaChatClient(_modelName);
        }

        public async Task<string> GetCategoryByPrompt(string userPrompt)
        {
            string categories = string.Join("\n", _modelTypes.Select(mt => mt.Type));
            string systemPrompt = "你是分類提示詞類型的助手，依提示詞的類型進行分類。分類有以下幾種，記得，只回應對應類型的問題類型，不要有其它的內容，例如只回應Coding。";
            systemPrompt = $"{systemPrompt}\n\n分類類型如下:\n{categories}";
            //string prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>";
            //var response = await _chatClient.GetResponseAsync(prompt);


            var messages = new List<ChatMessage>
            {
                new(Microsoft.Extensions.AI.ChatRole.System, systemPrompt),
                new(Microsoft.Extensions.AI.ChatRole.User, userPrompt)
            };
            var response = await _chatClient.GetResponseAsync(messages);


            string modelType = response.Text;

            return modelType;
        }

        public string GetModelName(string category)
        {
            var modelTarget = _modelTypes.FirstOrDefault(mt => mt.Type == category);
            return modelTarget?.ModelName ?? string.Empty;
        }

        public IChatClient GetChatClient(string modelName)
        {
            var modelTarget = _modelTypes.FirstOrDefault(mt => mt.ModelName == modelName);
            string provider = modelTarget?.Provider ?? "Ollama";

            return provider switch
            {
                "OpenAI" => CreateOpenAIChatClient(modelName),
                "AzureOpenAI" => CreateAzureOpenAIChatClient(modelName),
                _ => CreateOllamaChatClient(modelName)
            };
        }

        private IChatClient CreateOllamaChatClient(string modelName)
        {
            string endpoint = _config["Providers:Ollama:Endpoint"] ?? "http://localhost:11434";
            return new OllamaSharp.OllamaApiClient(new Uri(endpoint), modelName);
        }

        private IChatClient CreateOpenAIChatClient(string modelName)
        {
            string apiKey = _config["Providers:OpenAI:ApiKey"]
                ?? throw new InvalidOperationException("OpenAI API key is not configured in appsettings.json.");
            return new OpenAI.Chat.ChatClient(modelName, new ApiKeyCredential(apiKey)).AsIChatClient();
        }

        private IChatClient CreateAzureOpenAIChatClient(string modelName)
        {
            string endpoint = _config["Providers:AzureOpenAI:Endpoint"]
                ?? throw new InvalidOperationException("Azure OpenAI endpoint is not configured in appsettings.json.");
            string apiKey = _config["Providers:AzureOpenAI:ApiKey"]
                ?? throw new InvalidOperationException("Azure OpenAI API key is not configured in appsettings.json.");
            return new ChatCompletionsClient(
                new Uri(endpoint),
                new AzureKeyCredential(apiKey),
                new AzureAIInferenceClientOptions()
            ).AsIChatClient(modelName);
        }
    }
}
