using System;
using System.Collections.Generic;
using System.Dynamic;
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
            StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            dynamic jsonModel = System.Text.Json.JsonSerializer.Deserialize<ExpandoObject>(jsonStr);
            QdrantDbConfigModel? dbConfigModel = System.Text.Json.JsonSerializer.Deserialize<QdrantDbConfigModel>(jsonModel.QdrantDbConnection.ToString());
            return dbConfigModel;
        }
    }
}
