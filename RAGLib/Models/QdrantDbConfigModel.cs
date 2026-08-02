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
        public int Port { get; set; } 
        public string DeploymentName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = string.Empty;
        public ulong VectorSize { get; set; } 
        public string ApiKey { get; set; } = string.Empty;

        public static QdrantDbConfigModel? InitModel()
        {
            using StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            using var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonStr);
            var qdrantElement = jsonDoc.RootElement.GetProperty("QdrantDbConnection");
            QdrantDbConfigModel? dbConfigModel = System.Text.Json.JsonSerializer.Deserialize<QdrantDbConfigModel>(qdrantElement.GetRawText());
            return dbConfigModel;
        }
    }
}
