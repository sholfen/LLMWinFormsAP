using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp.Models;

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

        public Routing() : this(@"llama3.1:latest")
        {

        }

        public Routing(string modelName)
        {
            // 讀取 appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            if (string.IsNullOrEmpty(modelName))
            {
                _modelName = @"llama3.1:latest";
            }
            else
            {
                _modelName = modelName;
            }

            _modelTypes = config.GetSection("ModelTarget").GetChildren().Select(c => new ModelTarget
            {
                Type = c["Type"],
                ModelName = c["ModelName"]
            }).ToList();

            string ollamaUri = "http://localhost:11434";
            _chatClient = new OllamaSharp.OllamaApiClient(new Uri(ollamaUri), _modelName);
        }

        public async Task<string> GetCategoryByPrompt(string userPrompt)
        {
            
            string systemPrompt = "你是分類提示詞類型的助手，依提示詞的類型進行分類。分類有以下幾種，記得，只回應對應類型的問題類型，不要有其它的內容，例如只回應Coding。";
            systemPrompt += $"{systemPrompt}\n\n分類類型如下:\n{string.Join("\n", _modelTypes.Select(mt => $"{mt.Type}"))}";
            string prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>";
            var response = await _chatClient.GetResponseAsync(prompt);
            string modelType = response.Text;

            return modelType;
        }

        public string GetModelName(string category)
        {
            var modelTarget = _modelTypes.FirstOrDefault(mt => mt.Type == category);
            return modelTarget?.ModelName ?? string.Empty;
        }
    }
}
