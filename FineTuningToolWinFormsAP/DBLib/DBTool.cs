using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace FineTuningToolWinFormsAP.DBLib
{
    public class DBTool : IDisposable
    {
        //protected SqlConnection _sqlConnection { get; set; }
        protected MySqlConnection _sqlConnection { get; set; }

        public DBTool() 
        {
            using StreamReader sr = new StreamReader(@"Config.json");
            string jsonStr = sr.ReadToEnd();
            using var jsonDoc = System.Text.Json.JsonDocument.Parse(jsonStr);
            var connElement = jsonDoc.RootElement.GetProperty("ConnectionString");
            var localConn = connElement.GetProperty("Local").GetString();
            _sqlConnection = new MySqlConnection(localConn);
        }

        public void Dispose()
        {
            _sqlConnection?.Dispose();
        }

        public async Task InsertData(FineTuningBaseClassList fineTuningBaseClassList)
        {
            foreach (var item in fineTuningBaseClassList.messages)
            {
                string query =
                    $"INSERT INTO demo.FineTuningData (role,content) VALUES (@role,@content)";
                 await _sqlConnection.ExecuteAsync(query, item);
            }
        }

        public async Task<List<FineTuningBaseClassList>> Query()
        {       
            List<FineTuningBaseClassList> finalList = new List<FineTuningBaseClassList>();
            string sql = "SELECT * FROM demo.FineTuningData";
            var list = await _sqlConnection.QueryAsync<FineTuningBaseClass>(sql);
            int count = 0;
            FineTuningBaseClassList jsonLineResult = new FineTuningBaseClassList
            {
                messages = new List<FineTuningBaseClass>()
            };
            foreach (var item in list)
            {
                if (count >= 2)
                {
                    jsonLineResult.messages.Add(item);
                    finalList.Add(jsonLineResult);
                    jsonLineResult = new FineTuningBaseClassList
                    {
                        messages = new List<FineTuningBaseClass>()
                    };
                    count = 0;
                    continue;
                }
                jsonLineResult.messages.Add(item);
                count++;
            }
            return finalList;
        }
    }
}
