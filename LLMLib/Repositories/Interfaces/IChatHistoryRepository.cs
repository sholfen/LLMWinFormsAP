using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Interfaces
{
    public interface IChatHistoryRepository
    {
        void AddUserMessage(string toekn, string message);
        void AddAssistantMessage(string toekn, string message);
        List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory(string toekn);
        void ClearChatHistory(string toekn);
    }
}
