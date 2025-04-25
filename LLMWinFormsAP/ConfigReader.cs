using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    }

    public class ConfigReader
    {
        public ConfigReader(string path) 
        {
            //StreamReader sr = new StreamReader(path);
            //string jsonStr = sr.ReadToEnd();
            //var o = JsonSerializer.Deserialize(jsonStr);
        }
    }
}
