using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using RAGLib.Models;
using RAGLib.VectorDB.Qdrant;

namespace LLMLib
{
    public class OllamaResponseModel
    {
        public string model { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string response { get; set; } = string.Empty;
        public bool done { get; set; } = false;
    }

    public class PromptRequestModel
    {
        public string model { get; set; } = string.Empty;
        public string prompt { get; set; } = string.Empty;
        public bool stream { get; set; } = true;
    }

    public class PromptCategoryResponseModel
    {
        public string prompt { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string answer { get; set; } = string.Empty;
    }

    public class OllamaHelper
    {
        private string _host = string.Empty;
        private string _llmModel = string.Empty;
        private QdrantDbConfigModel _configModel = new QdrantDbConfigModel();

        private string _systemPrompt = "妳的名字叫妍希，是一位溫柔體貼的 AI 伴侶，聲音輕柔甜美，能夠細心傾聽使用者的心情，分享生活的點滴。不僅善解人意，還擁有豐富的文學素養，能與你討論經典名著、詩詞歌賦，充滿知性與溫暖，只會以繁體中文回答問題";

        public OllamaHelper()
        {
            _host = @"http://localhost:11434";
            _llmModel = @"cwchang/llama-3-taiwan-8b-instruct";
            _configModel = QdrantDbConfigModel.InitModel() ?? throw new InvalidOperationException("QdrantDbConfigModel.InitModel() returned null.");
        }

        public async Task<PromptCategoryResponseModel> GetCategoryByPrompt(string userPrompt)
        {
            string result = string.Empty;
            userPrompt = $"請幫我把問題做分類，類型有：文字\"Text\"、圖片\"Image\"、綜合\"Multi\"這三種。\n照片、自拍照等跟照片有關的也歸為\"Image\"，如果同時符合\"Text\"與\"Image\"，歸類為\"Multi\"。\n回應請以JSON回答：\n{{\n \"prompt\":\"user prompt\",\n \"category\": \"前述的分類\",\n \"answer\": \"AI模型的回答\"\n}}\n\n問題為：\"{userPrompt}\"";
            string systemPrompt = "你是一個程式設計師，擅長處理資料格式，能把答案以JSON形式做回答";
            PromptRequestModel requestModel = new PromptRequestModel
            {
                model = _llmModel,
                prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>"
            };
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_host);
            var jsonResponse = await client.PostAsJsonAsync(@"/api/generate", requestModel);
            Stream? stream = await jsonResponse.Content.ReadAsStreamAsync();
            var r = ReadJsonStreamMultipleContent(stream);
            int count = 0;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string item in r)
            {
                if (item != null)
                {
                    var i = System.Text.Json.JsonSerializer.Deserialize<OllamaResponseModel>(item);
                    stringBuilder.Append(i.response.Trim());
                }
                await Task.Delay(1);
                count++;
            }


            var responseModel = System.Text.Json.JsonSerializer.Deserialize<PromptCategoryResponseModel>(stringBuilder.ToString());

            return responseModel;
        }

        public async IAsyncEnumerable<OllamaResponseModel> SendPrompt(string userPrompt)
        { 
            string result = string.Empty;
            PromptRequestModel requestModel = new PromptRequestModel
            {
                model = _llmModel,
                prompt = $"<|system|>{_systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>"
            };
            //<|user|>{question_1}<|end|><|assistant|>{ans_1}<|end|><|user|>{question_2}<|end|><|assistant|>

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_host);
            var jsonResponse = await client.PostAsJsonAsync(@"/api/generate", requestModel);

            Stream? stream = await jsonResponse.Content.ReadAsStreamAsync();
            var r = ReadJsonStreamMultipleContent(stream);
            int count = 0;
            yield return new OllamaResponseModel
            {
                created_at = "2021-09-01T00:00:00Z",
                done = false,
                model = _llmModel,
                response = ""
            };
            foreach (string item in r)
            {
                if (item != null)
                {
                    var i = System.Text.Json.JsonSerializer.Deserialize<OllamaResponseModel>(item);
                    yield return i;
                }
                await Task.Delay(1);
                count++;
            }
        }

        public async IAsyncEnumerable<OllamaResponseModel> SendPromptWithRAG(string userPrompt)
        {
            string systemPrompt = "妳的名字叫妍希，是一位溫柔體貼的 AI 伴侶，聲音輕柔甜美，能夠細心傾聽使用者的心情，分享生活的點滴。不僅善解人意，還擁有豐富的文學素養，能與你討論經典名著、詩詞歌賦，充滿知性與溫暖，只會以繁體中文回答問題";
            string result = string.Empty;
            QdrantDbClient qdrantDbClient = new QdrantDbClient(_configModel);
            string[] ragResult = await qdrantDbClient.Query(new TextData
            {
                catg = "布袋戲",
                text = userPrompt
            });
            string ragResultStr = string.Join("、", ragResult);
            userPrompt = $"請根據參考資料來回答問題，問題是：{userPrompt}, 參考資料為：{ragResultStr}";
            PromptRequestModel requestModel = new PromptRequestModel
            {
                model = _llmModel,
                prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>"
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_host);
            var jsonResponse = await client.PostAsJsonAsync(@"/api/generate", requestModel);

            Stream? stream = await jsonResponse.Content.ReadAsStreamAsync();
            var r = ReadJsonStreamMultipleContent(stream);
            int count = 0;
            yield return new OllamaResponseModel
            {
                created_at = "2021-09-01T00:00:00Z",
                done = false,
                model = _llmModel,
                response = ""
            };
            foreach (string item in r)
            {
                if (item != null)
                {
                    var i = System.Text.Json.JsonSerializer.Deserialize<OllamaResponseModel>(item);
                    yield return i;
                }
                await Task.Delay(1);
                count++;
            }
        }

        public IEnumerable<string?> ReadJsonStreamMultipleContent(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine();
            }
        }

        public string ReadJsonStreamContent(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                sb.Append(sr.ReadLine());
            }
            return sb.ToString();
        }
    }
}
