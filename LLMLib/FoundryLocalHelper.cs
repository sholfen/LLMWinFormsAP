using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntimeGenAI;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib
{
    public class FoundryLocalHelper
    {
        private ICatalog _catalog;

        public async Task Demo()
        {
            try
            {
                CancellationToken ct = CancellationToken.None;
                Console.WriteLine("正在檢查環境與模型...");

                var modelAlias = "phi-4";
                var config = new Configuration
                {
                    AppName = "demo-Foundry",
                    LogLevel = Microsoft.AI.Foundry.Local.LogLevel.Information,
                    Web = new Configuration.WebService
                    {
                        Urls = "http://localhost:8888"
                    },
                };
                using var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                });
                var logger = loggerFactory.CreateLogger<FoundryLocalHelper>();
                await FoundryLocalManager.CreateAsync(config, logger, ct);
                if (FoundryLocalManager.IsInitialized)
                {
                    Console.WriteLine("Foundry Local Manager 初始化成功。");
                }
                else
                {
                    Console.WriteLine("Foundry Local Manager 初始化失敗。");
                    return;
                }


                var manager = FoundryLocalManager.Instance;
                _catalog = await manager.GetCatalogAsync(ct);
                IModel? model = await _catalog.GetModelAsync(modelAlias);
                //if (model == null)
                //{
                //    Console.WriteLine("找不到指定的模型，請確認模型別名是否正確，或是該模型是否已預載。");
                //    return;
                //}
                //Console.WriteLine($"模型資訊: {model.Alias} ({model.Id})");

                await model.DownloadAsync(progress =>
                {
                    Console.Write($"\rDownloading model: {progress:F2}%");
                    if (progress >= 100f)
                    {
                        Console.WriteLine();
                    }
                });
                //string path = await model.GetPathAsync();
                //Console.WriteLine($"模型路徑：{path}");

                //await model!.LoadAsync();
                //OpenAIChatClient chatClient = await model.GetChatClientAsync();



                await model.LoadAsync();
                await manager.StartWebServiceAsync(ct);

                IChatClient chatClient2 = new OpenAI.Chat.ChatClient(
                    modelAlias, new ApiKeyCredential("1234"),
                    new OpenAI.OpenAIClientOptions
                    {
                        Endpoint = new Uri("http://localhost:8888/v1")
                    }
                    ).AsIChatClient();

                string exitCommand = "exit";
                string userInput = "請你自我介紹";

                while (userInput != exitCommand)
                {
                    //var c=new ChatMessage( ChatRole.Assistant, "你現在是個3C達人，回答請用繁體中文");
                    List<ChatMessage> messages = new()
                    {
                        new ChatMessage(ChatRole.Assistant, "你現在是個3C達人，回答請用繁體中文"),
                        new ChatMessage(ChatRole.User, userInput)
                    };
                    var streamingResponse = chatClient2.GetStreamingResponseAsync(messages);
                    var response = await chatClient2.GetResponseAsync(messages);
                    Console.WriteLine(response.Text);
                    //await foreach (var chunk in streamingResponse)
                    //{    
                    //    Console.Write(chunk.Text);
                    //    Console.Out.Flush();
                    //}
                    Console.WriteLine();
                    Console.Write("請輸入下一個問題，或輸入 'exit' 結束對話：");
                    userInput = Console.ReadLine() ?? "";
                    if (userInput != exitCommand)
                    {
                        Console.WriteLine("對話結束");
                    }
                }

                // Tidy up - unload the model
                await model.UnloadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("啟動失敗。請確認：");
                Console.WriteLine($"錯誤訊息: {ex.Message}");
                Console.WriteLine($"詳細錯誤: {ex.StackTrace}");
            }
        }
    }
}
