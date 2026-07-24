

using CodingHelperConsoleAP;

string prompt = "寫一個C#的hello world範例";
LLMHelper helper = new LLMHelper();
string result = await helper.GetCodeAsync(prompt);
Console.WriteLine($"Prompt: {prompt}");
Console.WriteLine($"Generated Code:\n{result}");