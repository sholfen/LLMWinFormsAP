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
            _chatHistoryRepository = new LLMLib.Repositories.Implements.ChatHistoryRepository();
            // Initialize with a system message if needed
            _chatHistoryRepository.AddAssistantMessage("You are a helpful assistant.");   
        }

        public void AddUserMessage(string message)
        {
            _chatHistoryRepository.AddAssistantMessage(message);
        }
        public void AddAssistantMessage(string message)
        {
            _chatHistoryRepository.AddAssistantMessage(message);
        }
        public List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory()
        {
            return _chatHistoryRepository.GetChatHistory();
        }
        public void ClearChatHistory()
        {
            _chatHistoryRepository.ClearChatHistory();
        }
    }
}
