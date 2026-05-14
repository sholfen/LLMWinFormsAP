using Microsoft.Extensions.AI;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CodingHelperConsoleAP
{
    public class LLMHelper
    {
        private readonly string _modelName = "codegemma:7b-code-q8_0";
        private readonly IChatClient _chatClient;
        private List<ChatMessage> _messages = new List<ChatMessage>();

        public LLMHelper()
        {
            _chatClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), _modelName);
        }

        public LLMHelper(string modelName)
        {
            _modelName = modelName;
            _chatClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), _modelName);
        }

        public async Task<string> GetCodeAsync(string prompt)
        {
            ChatOptions chatOptions = new ChatOptions
            {
                ConversationId = Guid.NewGuid().ToString()
            };
            _messages.Add(new ChatMessage(ChatRole.User, prompt));
            var chatCompletion = await _chatClient.GetResponseAsync(_messages, chatOptions);
            return chatCompletion.Text;
        }
    }
}
