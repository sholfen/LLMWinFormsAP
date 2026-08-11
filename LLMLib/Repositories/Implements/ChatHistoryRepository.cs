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
        private ConcurrentDictionary<string, ConcurrentQueue<Microsoft.Extensions.AI.ChatMessage>> _chatHistoryDictionary = new ConcurrentDictionary<string, ConcurrentQueue< Microsoft.Extensions.AI.ChatMessage>>();

        public ChatHistoryRepository() 
        {

        }

        public void AddUserMessage(string token, string message)
        {
            var list = _chatHistoryDictionary.GetOrAdd(token, _ => new ConcurrentQueue<Microsoft.Extensions.AI.ChatMessage>());
            list.Enqueue(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.User, message));
        }
        public void AddAssistantMessage(string token, string message)
        {
            var list = _chatHistoryDictionary.GetOrAdd(token, _ => new ConcurrentQueue<Microsoft.Extensions.AI.ChatMessage>());
            list.Enqueue(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant, message));
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string token)
        {
            if (_chatHistoryDictionary.TryGetValue(token, out var list))
            {
                return list.ToList();
            }
            return new List<Microsoft.Extensions.AI.ChatMessage>();
        }
        public void ClearChatHistory(string token)
        {
            _chatHistoryDictionary.TryRemove(token, out _);
        }
    }
}
