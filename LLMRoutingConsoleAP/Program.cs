

using LLMRoutingLib;

Routing routing = new Routing();
string exitCommand = "exit";
string userInput = string.Empty;
while(userInput != exitCommand)
{
    Console.WriteLine("請輸入提示詞 (輸入 'exit' 以結束程式):");
    userInput = Console.ReadLine();
    if (string.IsNullOrEmpty(userInput) || userInput == exitCommand)
    {
        break;
    }
    string category = await routing.GetCategoryByPrompt(userInput);
    string modelName = routing.GetModelName(category);
    Console.WriteLine($"Result: Model: {modelName}, Category: {category}");
}
