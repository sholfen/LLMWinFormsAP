using LLMLib.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib.Repositories.Implements
{
    public class ChatHistoryMSSQLRepository: IChatHistoryRepository
    {

        private SqlConnection _sqlConnection;

        public ChatHistoryMSSQLRepository() 
        {
            _sqlConnection = new SqlConnection("YourConnectionStringHere");
        }

        public void AddAssistantMessage(string toekn, string message)
        {
            throw new NotImplementedException();
        }

        public void AddUserMessage(string toekn, string message)
        {
            throw new NotImplementedException();
        }

        public void ClearChatHistory(string toekn)
        {
            throw new NotImplementedException();
        }

        public List<ChatMessage> GetChatHistory(string toekn)
        {
            throw new NotImplementedException();
        }
    }
}
