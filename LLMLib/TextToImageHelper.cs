using RAGLib.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;

namespace LLMLib
{
    public class TextToImageHelper
    {
        private readonly HttpClient _httpClient;

        public static string Token { get; set; } = string.Empty;
        public static string EndPoint { get; set; } = string.Empty;

        public TextToImageHelper()
        {
            _httpClient = new HttpClient();  
        }

        public void Init()
        {
            StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            dynamic jsonModel = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
            LLMConfigModel? configModel = System.Text.Json.JsonSerializer.Deserialize<LLMConfigModel>(jsonModel.TextImage.ToString());
            Token = configModel.Token;
            EndPoint = configModel.EndPoint;
        }

        public async Task<string> GetImage(string prompt)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, EndPoint);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var requestModel = new
            {
                model = "Text-Image-Peter",
                prompt = prompt,
                n = 1,
                style = "vivid",
                quality = "standard",
                size = "1024x1024"
            };
            requestMessage.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.SendAsync(requestMessage);
            var stream = await response.Content.ReadAsStreamAsync();
            StreamReader streamReader = new StreamReader(stream);
            string jsonStr = streamReader.ReadToEnd();
            return jsonStr;
        }
    }
}
