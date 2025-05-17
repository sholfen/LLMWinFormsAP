using Azure.AI.OpenAI;
using Azure;
//using OpenAI.Embeddings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAGLib.Models;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Protobuf.WellKnownTypes;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Collections.Immutable;
using OpenAI.Embeddings;
using static System.Net.Mime.MediaTypeNames;
using System.Dynamic;

namespace RAGLib.VectorDB.Qdrant
{
    public class QdrantDbClient: IVectorDBClient
    {
        private readonly string _deploymentName;
        private readonly string _colName = "text_embedding";
        //private readonly EmbeddingClient _embeddingClient;
        private readonly QdrantClient _qdrantClient;
        private readonly string _ollamaHost = @"http://localhost:11434";

        public QdrantDbClient(QdrantDbConfigModel configModel)
        {
            _deploymentName = configModel.DeploymentName;
            _qdrantClient = new QdrantClient(configModel.Host, configModel.Port, false, configModel.ApiKey);
            if (!_qdrantClient.CollectionExistsAsync(_colName).Result)
            {
                _qdrantClient.CreateCollectionAsync(_colName,
                    new VectorParams { Size = configModel.VectorSize, Distance = Distance.Cosine }).Wait();
            }

            var count = _qdrantClient.CountAsync(_colName).Result;
        }

        public async Task InitData()
        {
            List<TextData> data = new List<TextData>
            {
                new TextData{ catg = "布袋戲", text = "清香白蓮素還真" },
                new TextData{ catg = "布袋戲", text = "半神半聖亦半仙，全儒全道是全賢，腦中真書藏萬卷，掌握文武半邊天" },
                new TextData{ catg = "布袋戲", text = "霹靂化身最多的首席男主角，溫文儒雅、器宇軒昂、超凡脫俗、武學莫測高深、足智多謀、博學多能、謙虛有禮，處世圓融冷靜、慈悲親和、關懷眾生；以武林和平、天下大同為己任，『謀為天下謀、利為天下利』 - 無我、無為！為武林風塵默默承受一切，多次以絕頂智慧化解災厄，置之死地而後生，為天下蒼生應現各種精彩玄奇的身份！幽默風趣的隨機教化，難捨能捨、忍辱負重、不計毀謗、無怨無悔，默默付出，不遺餘力、不求回報，真乃具足大慈悲與大智慧的凡聖一體，反璞歸真『素還真』。" },
                new TextData{ catg = "布袋戲", text = "日月星三才子" },
                new TextData{ catg = "詩詞", text = "床前明月光，疑是地上霜。舉頭望明月，低頭思故鄉。"},
                new TextData{ catg = "詩詞", text = "春眠不覺曉，處處聞啼鳥。夜來風雨聲，花落知多少。"},
                new TextData{ catg = "詩詞", text = "滿室天香仙子家，一琴一劍一杯茶。羽衣常帶煙霞色，不惹人間桃李花。"},
            };

            await InsertData(data);
        }

        public async Task<float[]> GetEmbeddings(TextData textData)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(_ollamaHost);
            var requestModel = new
            {
                model = @"tazarov/all-minilm-l6-v2-f32",
                input = textData.text
            };
            var jsonResponse = await httpClient.PostAsJsonAsync(@"/api/embed", requestModel);
            Stream? stream = await jsonResponse.Content.ReadAsStreamAsync();
            StreamReader sr = new StreamReader(stream);
            string jsonStr = sr.ReadToEnd();
            var embeddingResult = System.Text.Json.JsonSerializer.Deserialize<EmbeddingResult>(jsonStr);
            Console.WriteLine($"Printing embedding result: {textData.text}");
            int count = 0;
            foreach (var vectors in embeddingResult.embeddings)
            {
                foreach (var vector in vectors)
                {
                    Console.Write($"{vector} ");
                }
                count++;
            }
            Console.WriteLine();
            Console.WriteLine($"Printing embedding result count: {count}");
            Console.WriteLine();
            return embeddingResult.embeddings[0];
        }

        public float[] GetEmbeddingsByAzure(string text)
        {
            StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            dynamic jsonModel = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
            AzureConfigModel? azureConfigModel = System.Text.Json.JsonSerializer.Deserialize<AzureConfigModel>(jsonModel.AzureAPI.ToString());


            var endpoint = new Uri(azureConfigModel.Host);
            var apiKey = azureConfigModel.ApiKey;

            var credential = new AzureKeyCredential(apiKey);
            AzureKeyCredential credentials = new(apiKey);

            AzureOpenAIClient azureOpenAIClient = new AzureOpenAIClient(endpoint, credentials);
            var embeddingClient = azureOpenAIClient.GetEmbeddingClient(_deploymentName);

            var r = embeddingClient.GenerateEmbedding(text);
            var embedding = r.Value;
            ReadOnlyMemory<float> vec = embedding.ToFloats();
            return vec.ToArray();
        }

        public async Task InsertData(List<TextData> textList)
        {
            var points = textList.Select(t =>
            {
                var catg = t.catg;
                var text = t.text;
                PointId id = new PointId();
                id.Uuid = Guid.NewGuid().ToString();
                return new PointStruct
                {

                    Id = id,
                    Vectors = GetEmbeddings(t).Result,
                    //Vectors = GetEmbeddingsByAzure(t.text),
                    Payload =
                    {
                            ["catg"] = catg,
                            ["text"] = text
                    }
                };
            }).ToList();
            var updateResult = await _qdrantClient.UpsertAsync(_colName, points);
        }

        public async Task<string[]> Query(TextData textData)
        {
            //var client = new QdrantClient("localhost", 6334, false, "3065678qazwsx");
            string keywd = textData.text;
            var queryVector = await GetEmbeddings(textData);
            //var queryVector =   GetEmbeddingsByAzure(textData.text);
            var answers = await _qdrantClient.SearchAsync(
                _colName,
                queryVector,
                filter: Conditions.MatchText("catg", textData.catg),
                limit: 3);    
            if (answers == null)
            {
                return Array.Empty<string>();
            }
            List<string> result = new List<string>();
            foreach (var item in answers)
            {
                var text = item.Payload["text"].StringValue;
                result.Add(text);
            }
            return result.ToArray();
        }
    }
}
