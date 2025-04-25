using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAGLib.Models
{
    public class QdrantDbConfigModel
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 6334;
        public string DeploymentName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
        public int VectorSize { get; set; } = 1536;
        public string ApiKey { get; set; } = string.Empty;

        public static QdrantDbConfigModel? InitModel()
        {
            StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            return System.Text.Json.JsonSerializer.Deserialize<QdrantDbConfigModel?>(jsonStr);   
        }
    }
}
