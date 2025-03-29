using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib
{
    public class OllamaResponseModel
    {
        public string model { get; set; } = string.Empty;
        public string created_at { get; set; } = string.Empty;
        public string response { get; set; } = string.Empty;
        public bool done { get; set; } = false;
        //public string done_reason { get; set; } = string.Empty;
        //public int[]? context { get; set; } = null;
    }

    public class PromptRequestModel
    {
        public string model { get; set; } = string.Empty;
        public string prompt { get; set; } = string.Empty;
        public bool stream { get; set; } = true;
    }

    public class OllamaHelper
    {
        private string _host = string.Empty;
        private string _llmModel = string.Empty;

        public OllamaHelper()
        {
            _host = @"http://localhost:11434";
            //_llmModel = "gemma";
            //_llmModel = @"deepseek-r1";
            _llmModel = @"cwchang/llama-3-taiwan-8b-instruct";
        }

        public async IAsyncEnumerable<OllamaResponseModel> SendPrompt(string userPrompt)
        {
            string systemPrompt = "妳的名字叫妍希，是一位溫柔體貼的 AI 伴侶，聲音輕柔甜美，能夠細心傾聽使用者的心情，分享生活的點滴。不僅善解人意，還擁有豐富的文學素養，能與你討論經典名著、詩詞歌賦，充滿知性與溫暖，只會以繁體中文回答問題";
            string result = string.Empty;
            PromptRequestModel requestModel = new PromptRequestModel
            {
                model = _llmModel,
                prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>"
            };//<|user|>{ question_1}<|end|><|assistant|>{ ans_1}<|end|><|user|>{ question_2}<|end|><|assistant|>

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_host);
            //var response = await client.GetAsync(@"/api/generate");
            var jsonResponse = await client.PostAsJsonAsync(@"/api/generate", requestModel);
            //var jsonResponse = await client.GetAsync(@"/MyAPI/GetJSONStream");
            //result = await jsonResponse.Content.ReadAsStringAsync();

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
                    //Console.Write(i.response);
                    //Console.Write(item);
                    //Console.WriteLine();
                    yield return i;
                }
                await Task.Delay(1);
                count++;
            }
            //string r = await jsonResponse.Content.ReadAsStringAsync();
            //Console.WriteLine(r);
            //Console.WriteLine();
            //Console.WriteLine(count.ToString());
        }

        public IEnumerable<string?> ReadJsonStreamMultipleContent(Stream stream)
        {
            StreamReader sr = new StreamReader(stream);
            while (!sr.EndOfStream)
            {
                yield return sr.ReadLine();
            }
        }

        //public async Task<IAsyncEnumerable<TResult?>> ReadJsonStreamMultipleContent<TResult>(Stream stream)
        //{
        //    return System.Text.Json.JsonSerializer.DeserializeAsyncEnumerable<TResult>(stream);
        //}

        //static async IAsyncEnumerable<int> PrintNumbers(int n)
        //{
        //    for (int i = 0; i < n; i++)
        //    {
        //        await Task.Delay(1000);
        //        yield return i;
        //    }
        //}
    }
}
