using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Interfaces
{
    public interface IChatHistoryRepository
    {
        void AddUserMessage(string token, string message);
        void AddAssistantMessage(string token, string message);
        List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string token);
        void ClearChatHistory(string token);
    }
}
