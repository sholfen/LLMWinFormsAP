// See https://aka.ms/new-console-template for more information

using LLMLib;
using RAGLib.Models;
using SearchEngineManager;
using System.Dynamic;

//skill加上一條：如果專案為Console App，則在第一行加上：Console.OutputEncoding = System.Text.Encoding.UTF8;

//StreamReader sr = new StreamReader(@"Config.json");
//string jsonStr = sr.ReadToEnd();
//dynamic jsonModel = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
//AzureConfigModel? azureConfigModel = System.Text.Json.JsonSerializer.Deserialize<AzureConfigModel>(jsonModel.AzureAPI.ToString());

LLMExtensionHelper lLMExtensionHelper = new LLMExtensionHelper();
//LLMExtensionHelper.Host = azureConfigModel.Host;
//LLMExtensionHelper.ApiKey = azureConfigModel.ApiKey;
//await lLMExtensionHelper.AIChatTest();
//await lLMExtensionHelper.AIChatTest5();
//await lLMExtensionHelper.AIChatTest4();
await lLMExtensionHelper.AIChatTest6();


//FoundryLocalHelper foundryLocalHelper = new FoundryLocalHelper();
//await foundryLocalHelper.Demo();


public class AzureConfigModel
{
    public string Host { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}