// See https://aka.ms/new-console-template for more information

using LLMLib;
using RAGLib.Models;
using System.Dynamic;

void SomeTest()
{
    string sql = """
    select name, age from users where age > 30 order by age desc;
    delete from users where age < 20;
    SELECT department, COUNT(*) as employee_count
    FROM employees
    WHERE hire_date >= '2020-01-01';
    """;
    Console.WriteLine($"SQL:");
    Console.WriteLine(sql);
}

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
await lLMExtensionHelper.AIChatTest5();
//await lLMExtensionHelper.AIChatTest4();


public class AzureConfigModel
{
    public string Host { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}