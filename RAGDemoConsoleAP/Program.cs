
using LLMLib;
using LLMLib.Models;
using RAGLib.Models;
using RAGLib.VectorDB.Qdrant;
using System.Net;
using System.Net.Http;

//var configModel = QdrantDbConfigModel.InitModel();
//QdrantDbClient qdrantDbClient = new QdrantDbClient(configModel);
//await qdrantDbClient.InitData();
//string prompt= "床前明月光完整的詩是什麼";
//TextData textData = new TextData
//{
//    catg = string.Empty,
//    text = prompt
//};
//Console.WriteLine(prompt);
//string[] response = await qdrantDbClient.Query(textData);
//int count = 1;
//foreach (var item in response)
//{
//    Console.WriteLine($"{count}.{item}");
//    count++;
//}

TextToImageHelper textToImageHelper = new TextToImageHelper();
textToImageHelper.Init();
string prompt = "真人影像，一個面帶微笑的女高中生，在外面逛街，而且外面天氣很好";
string response = await textToImageHelper.GetImage(prompt);
Console.WriteLine(response);

ImageResult? imageResult = System.Text.Json.JsonSerializer.Deserialize<ImageResult>(response);
if (imageResult != null && (imageResult.data != null && imageResult.data.Count() != 0))
{
    Console.WriteLine($"Revised Prompt: {imageResult.data[0].revised_prompt}");
    Console.WriteLine($"Image URL: {imageResult.data[0].url}");

    string fileName = $"{DateTime.Now:yyyyMMddHHmmss}.png";
    using HttpClient httpClient = new HttpClient();
    using var downloadStream = await httpClient.GetStreamAsync(imageResult.data[0].url);
    using var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
    await downloadStream.CopyToAsync(fileStream);
    await fileStream.FlushAsync();
}
else
{
    Console.WriteLine("Failed to deserialize the response.");
}