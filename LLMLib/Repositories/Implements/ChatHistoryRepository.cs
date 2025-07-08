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
        private List<Microsoft.Extensions.AI.ChatMessage> _chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();
        private ConcurrentDictionary<string, List<Microsoft.Extensions.AI.ChatMessage>> _chatHistoryDictionary = new ConcurrentDictionary<string, List<Microsoft.Extensions.AI.ChatMessage>>();

        public ChatHistoryRepository() 
        {

        }

        public void AddUserMessage(string toekn,string message)
        {
            _chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.User, message));
        }
        public void AddAssistantMessage(string toekn, string message)
        {
            _chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant, message));
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string toekn)
        {
            if (_chatHistoryDictionary.ContainsKey(toekn))
            {
                return _chatHistoryDictionary[toekn].ToList();
            }
            return new List<Microsoft.Extensions.AI.ChatMessage>();
            //return _chatHistory;
        }
        public void ClearChatHistory(string toekn)
        {
            //_chatHistory.Clear();
            if(_chatHistoryDictionary.ContainsKey(toekn))
            {

            }
        }
    }
}
