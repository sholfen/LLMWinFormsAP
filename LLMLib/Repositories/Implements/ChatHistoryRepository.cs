using LLMLib.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Implements
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private ConcurrentDictionary<string, List<Microsoft.Extensions.AI.ChatMessage>> _chatHistoryDictionary = new ConcurrentDictionary<string, List<Microsoft.Extensions.AI.ChatMessage>>();

        public ChatHistoryRepository() 
        {

        }

        public void AddUserMessage(string token, string message)
        {
            if (!_chatHistoryDictionary.ContainsKey(token))
            {
                _chatHistoryDictionary[token] = new List<Microsoft.Extensions.AI.ChatMessage>();
            }
            _chatHistoryDictionary[token].Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.User, message));
        }
        public void AddAssistantMessage(string token, string message)
        {
            if (!_chatHistoryDictionary.ContainsKey(token))
            {
                _chatHistoryDictionary[token] = new List<Microsoft.Extensions.AI.ChatMessage>();
            }
            _chatHistoryDictionary[token].Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant, message));
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string token)
        {
            if (_chatHistoryDictionary.ContainsKey(token))
            {
                return _chatHistoryDictionary[token].ToList();
            }
            return new List<Microsoft.Extensions.AI.ChatMessage>();
        }
        public void ClearChatHistory(string token)
        {
            if(_chatHistoryDictionary.ContainsKey(token))
            {
                _chatHistoryDictionary.TryRemove(token, out _);
            }
        }
    }
}
