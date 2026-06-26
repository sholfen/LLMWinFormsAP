using Microsoft.Extensions.AI;
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

        public Routing() : this(@"llama3.1:latest")
        {

        }

        public Routing(string modelName)
        {
            _modelName = modelName;
            _chatClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), _modelName);
        }

        public string GetModelName(string prompt)
        {
            string systemPrompt = "You are a helpful assistant.";

            return _modelName;
        }
    }
}
