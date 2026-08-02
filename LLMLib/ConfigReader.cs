using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace LLMWinFormsAP
{
    public class ConfigClass
    {
        public GirlItem[] Girls { get; set; }
    }

    public class GirlItem
    {
        public string Name { get; set; } = string.Empty;
        public string[] Systems { get; set; } = Array.Empty<string>();
    }

    public class ConfigReader
    {
        private readonly ConfigClass _config;

        public ConfigReader(string path)
        {
            using StreamReader sr = new StreamReader(path);
            string jsonStr = sr.ReadToEnd();
            _config = JsonSerializer.Deserialize<ConfigClass>(jsonStr)
                      ?? throw new InvalidOperationException("Failed to deserialize configuration.");
        }

        public GirlItem[] GetGirls()
        {
            return _config.Girls;
        }
    }
}
