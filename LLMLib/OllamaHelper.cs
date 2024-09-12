using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public class OllamaHelper
    {
        private string _host = string.Empty;
        private string _llmModel = string.Empty;

        public OllamaHelper()
        {
            _host = @"http://localhost:11434";
            //_host = @"http://localhost:5000";
            //_llmModel = "gemma";
            _llmModel = @"cwchang/llama-3-taiwan-8b-instruct";
        }

        public async IAsyncEnumerable<OllamaResponseModel> SendPrompt(string userPrompt)
        {
            string systemPrompt = "You are a knowledgeable and friendly assistant. Answer the following question as clearly and concisely as possible, providing any relevant information and examples.";
            string result = string.Empty;
            PromptRequestModel requestModel = new PromptRequestModel
            {
                model = _llmModel,
                prompt = $"<|system|>{systemPrompt}<|end|><|user|>{userPrompt}<|end|><|assistant|>"
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_host);
            //var response = await client.GetAsync(@"/api/generate");
            var jsonResponse = await client.PostAsJsonAsync(@"/api/generate", requestModel);
            //var jsonResponse = await client.GetAsync(@"/MyAPI/GetJSONStream");
            //result = await jsonResponse.Content.ReadAsStringAsync();

            Stream? stream = await jsonResponse.Content.ReadAsStreamAsync();
            var r = ReadJsonStreamMultipleContent(stream);
            int count = 0;
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
