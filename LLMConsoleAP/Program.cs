// See https://aka.ms/new-console-template for more information

using LLMLib;
using RAGLib.Models;
using System.Dynamic;
/*
string prompt = "女僕咖啡廳提供什麼服務";
//Console.Write("輸入提示詞：");
//prompt = Console.ReadLine();
Console.WriteLine($"Prompt: {prompt}");
OllamaHelper ollamaHelper = new OllamaHelper();
await foreach (var item in ollamaHelper.SendPrompt(prompt))
{
    Console.Write(item.response);
}
Console.WriteLine();
Console.WriteLine("Pro End.");
*/

StreamReader sr = new StreamReader(@"Config.json");
string jsonStr = sr.ReadToEnd();
dynamic jsonModel = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
AzureConfigModel? azureConfigModel = System.Text.Json.JsonSerializer.Deserialize<AzureConfigModel>(jsonModel.AzureAPI.ToString());

LLMExtensionHelper lLMExtensionHelper = new LLMExtensionHelper();
LLMExtensionHelper.Host = azureConfigModel.Host;
LLMExtensionHelper.ApiKey = azureConfigModel.ApiKey;
//await lLMExtensionHelper.AIChatTest();
await lLMExtensionHelper.AIChatTest2();

public class AzureConfigModel
{
    public string Host { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}