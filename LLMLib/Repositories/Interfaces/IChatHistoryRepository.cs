using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Interfaces
{
    public interface IChatHistoryRepository
    {
        void AddUserMessage(string message);
        void AddAssistantMessage(string message);
        List<Microsoft.Extensions.AI.ChatMessage> GetChatHistory();
        void ClearChatHistory();
    }
}
