
using RAGLib.Models;
using RAGLib.VectorDB.Qdrant;

var configModel = QdrantDbConfigModel.InitModel();
QdrantDbClient qdrantDbClient = new QdrantDbClient(configModel);
//await qdrantDbClient.InitData();
string prompt= "床前明月光完整的詩是什麼";
TextData textData = new TextData
{
    catg = string.Empty,
    text = prompt
};
Console.WriteLine(prompt);
string[] response = await qdrantDbClient.Query(textData);
int count = 1;
foreach (var item in response)
{
    Console.WriteLine($"{count}.{item}");
    count++;
}