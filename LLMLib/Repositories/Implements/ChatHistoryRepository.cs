using LLMLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Implements
{
    public class ChatHistoryRepository : IChatHistoryRepository
    {
        private List<Microsoft.Extensions.AI.ChatMessage> _chatHistory = new List<Microsoft.Extensions.AI.ChatMessage>();

        public ChatHistoryRepository() 
        {

        }

        public void AddUserMessage(string message)
        {
            _chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.User, message));
        }
        public void AddAssistantMessage(string message)
        {
            _chatHistory.Add(new Microsoft.Extensions.AI.ChatMessage(Microsoft.Extensions.AI.ChatRole.Assistant, message));
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory()
        {
            return _chatHistory;
        }
        public void ClearChatHistory()
        {
            _chatHistory.Clear();
        }
    }
}
