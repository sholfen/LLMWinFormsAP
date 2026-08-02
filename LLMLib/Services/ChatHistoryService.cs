using LLMLib.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Services
{
    public class ChatHistoryService
    {
        private IChatHistoryRepository _chatHistoryRepository;

        public ChatHistoryService()
        {
            _chatHistoryRepository = new Repositories.Implements.ChatHistoryRepository();
            //_chatHistoryRepository.AddAssistantMessage(string.Empty, "You are a helpful assistant.");
        }

        public void AddUserMessage(string token, string message)
        {
            _chatHistoryRepository.AddUserMessage(token, message);
        }
        public void AddAssistantMessage(string token, string message)
        {
            _chatHistoryRepository.AddAssistantMessage(token, message);
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string token)
        {
            return _chatHistoryRepository.GetChatHistory(token);
        }
        public void ClearChatHistory(string token)
        {
            _chatHistoryRepository.ClearChatHistory(token);
        }
    }
}
