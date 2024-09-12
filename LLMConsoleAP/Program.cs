// See https://aka.ms/new-console-template for more information

using LLMLib;

string prompt = "女僕咖啡廳提供什麼服務";
Console.Write("輸入提示詞：");
prompt = Console.ReadLine();
Console.WriteLine($"Prompt: {prompt}");
OllamaHelper ollamaHelper = new OllamaHelper();
await foreach (var item in ollamaHelper.SendPrompt(prompt))
{
    Console.Write(item.response);
}
Console.WriteLine("Pro End.");
