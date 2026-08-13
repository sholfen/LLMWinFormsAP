

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
    dynamic? categoryObj = System.Text.Json.JsonSerializer.Deserialize<dynamic>(category);
    if(categoryObj == null ) throw new Exception("Failed to deserialize category response.");
    string modelName = routing.GetModelName(categoryObj.category.ToString());
    Console.WriteLine($"Result: Model: {modelName}, Category: {categoryObj.category}");
}
