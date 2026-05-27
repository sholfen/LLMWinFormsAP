using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.ML.OnnxRuntimeGenAI;
using OpenAI.Chat;
using System.Text;

namespace LLMLib
{
    public class AgentMethodClass
    {
        public static int GetStockPrice(string stockSymbol)
        {
            // 模擬獲取股票價格的邏輯
            Random random = new Random();
            return random.Next(100, 500); // 返回隨機的股票價格
        }
    }

    public class LLMHelper
    {
        private string _modelPath = string.Empty;
        private Thread _thread;
        private Model? _model;

        public LLMHelper(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path is invalid.");
            }
            SetModelPath(path);
        }

        public LLMHelper()
        {
            SetModelPath("D:\\LLM\\onnx\\Phi-3-mini-4k-instruct-onnx\\cuda\\cuda-fp16");
        }

        private void SetModelPath(string modelPath)
        {
            _modelPath = modelPath;
        }

        public void Load()
        {
            _model = new Model(_modelPath);
        }

        public void ThreadStop()
        {
            if (_thread != null && _thread.IsAlive)
            {
                _thread.Interrupt();
                _thread.Join();
            }
        }

        // Microsoft.Agents.AI 套件範例方法
        // 注意：請先安裝 Microsoft.Agents.AI NuGet 套件
        public async Task RunAgentsAISampleAsync()
        {
            string model = @"llama3.1:latest";
            IChatClient chatClient = new OllamaSharp.OllamaApiClient(new Uri("http://localhost:11434"), model);
            AIAgent agent = new ChatClientAgent(
                chatClient,
                new ChatClientAgentOptions
                {
                    Name = "llama3.1:latest",
                    //Instructions = "你現在是個笑話專家。",
                    Description = "一個可以講海盜笑話的笑話專家代理人。",
                });

            var aiFunctions = new List<AITool>();
            aiFunctions.Add(AIFunctionFactory.Create(AgentMethodClass.GetStockPrice));
            IChatClient clientWithTools = new FunctionInvokingChatClient(chatClient)
            {
                AdditionalTools = aiFunctions
            };

            await foreach (var update in agent.RunStreamingAsync("有沒有AI的笑話可以講？"))
            {
                Console.Write(update);
            }

            await foreach (var updatee in clientWithTools.GetStreamingResponseAsync("請幫我查詢一下AAPL的股票價格。"))
            {
                Console.Write(updatee);
            }
        }
    }
}
